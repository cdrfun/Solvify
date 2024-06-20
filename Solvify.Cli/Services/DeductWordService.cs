using Solvify.Cli.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solvify.Cli.Services;

public class DeductWordService
{
    public enum GuessingResult
    {
        Win,
        Processed,
        InvalidCharacter,
        InvalidLength,
        InvalidWord
    }

    private const char InvalidWordChar = 'x';
    private const char InWordChar = '+';
    private const char NoMatchChar = '-';
    private const char PositionalMatchChar = '*';

    private readonly List<string> _invalidWordList = [];
    private readonly Dictionary<int, char> _inWord = [];
    private readonly Dictionary<int, char> _positionalMatches = [];
    private readonly WordScoringService _scoringService;
    private readonly string _validCharacters;
    private readonly int _wordLength;
    private readonly List<string> _wordlist;

    private ScoredWord? _currentGuess;
    private string _noMatch = string.Empty;

    public DeductWordService(int wordLength, List<string> wordlist, string validCharacters)
    {
        _validCharacters = validCharacters;
        _scoringService = new WordScoringService(wordlist.Where(x => x.Length == wordLength), validCharacters);
        _wordLength = wordLength;
        _wordlist = wordlist;
    }
    
    public int GetGuessCount { get; private set; }

    public GuessingResult GetLastGuessingResult { get; private set; } = GuessingResult.Processed;
    
    /// <summary>
    ///     Add and process the result of the current guess
    /// </summary>
    /// <param name="guessingResult"></param>
    /// <returns>True if guessingResult was valid and processed correctly</returns>
    public GuessingResult AddCurrentGuessResult(string guessingResult)
    {
        GetLastGuessingResult = GetCurrentGuessResult(guessingResult);
        return GetLastGuessingResult;
    }

    public ScoredWord GetCurrentGuess()
    {
        if (_currentGuess is not null)
        {
            return _currentGuess;
        }

        string noScore = GetNoScoreCharacters();

        IEnumerable<string> activeWords = GetActiveWords();

        List<ScoredWord> guessList = activeWords.Select(word => new ScoredWord
            {
                Word = word, Score = _scoringService.GetScore(word, noScore)
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Word)
            .ToList();

        ScoredWord guess = guessList.FirstOrDefault()
                           ?? new ScoredWord
                           {
                               Score = 0, Word = string.Empty
                           }; // I have no word, maybe throw exception?

        _currentGuess = guess;
        return _currentGuess;
    }

    /// <summary>
    ///     Get a human-readable message based on the last guessing result
    /// </summary>
    /// <returns>Message for last guessing result</returns>
    public string GetLastGuessingResultMessage()
    {
        return GetLastGuessingResult switch
        {
            GuessingResult.Win => $"You won! It took us {GetGuessCount} guesses to solve the puzzle",
            GuessingResult.Processed => $"Processed guess {GetGuessCount}",
            GuessingResult.InvalidCharacter => "Invalid character entered in result",
            GuessingResult.InvalidLength => "Result must match length of guessed word",
            GuessingResult.InvalidWord => "Damn!",
            _ => string.Empty
        };
    }
    
    private Regex CreateRegexPattern()
    {
        string regex = string.Empty;

        foreach (KeyValuePair<int, char> i in _inWord)
        {
            if (_positionalMatches.All(m => m.Value != i.Value))
            {
                regex += $"(?=.*{i.Value})";
            }
        }

        for (int i = 0; i < _wordLength; i++)
        {
            if (_positionalMatches.TryGetValue(i, out char posMatch))
            {
                regex += posMatch;
            }
            else
            {
                regex += $"[^{_noMatch}";

                if (_inWord.TryGetValue(i, out char notHereChar))
                {
                    regex += $"{notHereChar}";
                }

                regex += "]";
            }
        }

        return new Regex(regex, RegexOptions.IgnoreCase);
    }

    private IEnumerable<string> GetActiveWords()
    {
        IEnumerable<string> activeWords = _wordlist.Where(x => x.Length == _wordLength)
            .Where(x => !_invalidWordList.Contains(x))
            .Where(x => x.All(c => _validCharacters.Contains(c, StringComparison.OrdinalIgnoreCase)));

        if (GetGuessCount <= 0)
        {
            return activeWords;
        }

        Regex regex = CreateRegexPattern();
        activeWords = activeWords.Where(x => regex.IsMatch(x));

        return activeWords;
    } 

    private GuessingResult GetCurrentGuessResult(string guessingResult)
    {
        string guess = _currentGuess?.Word
                       ?? GetCurrentGuess()
                           .Word;

        if (guessingResult == InvalidWordChar.ToString())
        {
            _invalidWordList.Add(guess);
            _currentGuess = null;
            return GuessingResult.InvalidWord;
        }

        if (guessingResult.Length != guess.Length)
        {
            return GuessingResult.InvalidLength;
        }

        if (guessingResult.Any(x => x is not PositionalMatchChar and not InWordChar and not NoMatchChar))
        {
            return GuessingResult.InvalidCharacter;
        }

        if (guessingResult.All(x => x == PositionalMatchChar))
        {
            GetGuessCount++;
            return GuessingResult.Win;
        }

        ProcessGuessingResult(guessingResult, guess);

        return GuessingResult.Processed;
    }

    private string GetNoScoreCharacters()
    {
        string noScore = string.Empty;

        foreach (KeyValuePair<int, char> positionalMatch in _positionalMatches)
        {
            noScore += positionalMatch.Value;
        }

        return noScore;
    }

    private void ProcessGuessingResult(string guessingResult, string guess)
    {
        GetGuessCount++;

        for (int i = 0; i < guessingResult.Length; i++)
        {
            switch (guessingResult[i])
            {
                case PositionalMatchChar:
                    _ = _positionalMatches.TryAdd(i, guess[i]);
                    break;

                case InWordChar:
                    _ = _inWord.TryAdd(i, guess[i]);
                    break;

                case NoMatchChar:
                    _noMatch += guess[i];
                    break;
            }
        }

        _currentGuess = null;
    }
}