using Solvify.Cli.Records;
using Solvify.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solvify.Cli.Services;

public class DeductWordService
{
    private readonly SolvifySettings _solvifySettings;
    private readonly GameSettings _gameSettings;
    private char InWordChar => _solvifySettings.InWordChar;
    private char InvalidWordChar => _solvifySettings.InvalidWordChar;
    private char NoMatchChar => _solvifySettings.NoMatchChar;
    private char PositionalMatchChar => _solvifySettings.PositionalMatchChar;


    public DeductWordService(SolvifySettings solvifySettings, GameSettings gameSettings, List<string> wordlist)
    {
        _solvifySettings = solvifySettings;
        _matchingChars = string.Concat(InWordChar, NoMatchChar, PositionalMatchChar);
        _gameSettings = gameSettings;
        _wordlist = wordlist;
        _scoringService = new WordScoringService(wordlist.Where(x => x.Length == _gameSettings.GuessLength), _gameSettings.ValidCharacters);
    }

    private readonly List<GuessedCharacter> _guessingResultsDictionary = [];

    private readonly List<string> _invalidWordList = [];
    private string _matchingChars;
    private readonly WordScoringService _scoringService;
    private readonly string _validCharacters;
    private readonly int _wordLength;
    private readonly List<string> _wordlist;

    private ScoredWord? _currentGuess;

    public string InconsistentGuessingResultMessage { get; set; } = string.Empty;

    public int GetGuessCount { get; private set; }
    public int ActiveWordsOfLastGuess { get; private set; }

    public GuessingResult GetLastGuessingResult { get; private set; } = GuessingResult.Processed;

    private List<(char Character, char GuessingResultCharacter)> PositionalCharacterList => _guessingResultsDictionary
        .Where(x => x.GuessingResultCharacter == PositionalMatchChar)
        .Select(x => (x.Character, x.GuessingResultCharacter))
        .Distinct()
        .ToList();

    private List<(char Character, char GuessingResultCharacter)> InWordCharacterList => _guessingResultsDictionary
        .Where(x => x.GuessingResultCharacter == InWordChar)
        .Select(x => (x.Character, x.GuessingResultCharacter))
        .Distinct()
        .ToList();

    public string MatchingChars => _matchingChars;

    /// <summary>
    ///     Add and process the result of the current guess
    /// </summary>
    /// <param name="guessingResult"></param>
    /// <returns>True if guessingResult was valid and processed correctly</returns>
    public GuessingResult AddCurrentGuessResult(string guessingResult)
    {
        GetLastGuessingResult = EvaluateGuessingResult(guessingResult);
        return GetLastGuessingResult;
    }

    public ScoredWord GetCurrentGuess()
    {
        if (_currentGuess is not null)
        {
            return _currentGuess;
        }

        string noScore = GetNoScoreCharacters();

        List<string> activeWords = GetActiveWords().ToList();

        ActiveWordsOfLastGuess = activeWords.Count();
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
        // Create lookahead for all characters in InWord which are not already in PositionalMatches
        string regex = string.Empty;
        regex = InWordCharacterList.Where(x => !PositionalCharacterList.Contains(x))
            .Aggregate(regex, (current, next) => current + $"(?=.*{next.Character})");

        // Create match pattern for each character in the word
        for (int i = 0; i < _wordLength; i++)
        {
            // For positional matches, just search for the exact character
            int position = i;
            char positionalMatch = char.Parse(_guessingResultsDictionary
                .Where(x => x.Position == position && x.GuessingResultCharacter == PositionalMatchChar)
                .Select(x => x.Character.ToString()).Distinct(StringComparer.CurrentCultureIgnoreCase)
                .SingleOrDefault() ?? char.MinValue.ToString());
            if (positionalMatch != char.MinValue)
            {
                regex += positionalMatch;
            }
            else
            {
                string searchNotFor = string.Empty;
                searchNotFor = _guessingResultsDictionary.Where(x =>
                        x.GuessingResultCharacter == NoMatchChar ||
                        (x.GuessingResultCharacter == InWordChar && x.Position == position)).Distinct()
                    .Aggregate(searchNotFor, (current, next) => current + next.Character);

                regex += $"[^{searchNotFor}]";
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

    private GuessingResult EvaluateGuessingResult(string guessingResult)
    {
        string guess = _currentGuess?.Word
                       ?? GetCurrentGuess()
                           .Word;

        // User said the word is invalid
        if (guessingResult == InvalidWordChar.ToString())
        {
            _invalidWordList.Add(guess);
            _currentGuess = null;
            return GuessingResult.InvalidWord;
        }

        // User entered a result not matching the lenght of the guess
        if (guessingResult.Length != guess.Length)
        {
            return GuessingResult.InvalidLength;
        }

        // User entered an invalid character as guessing result
        if (guessingResult.Any(x => x != PositionalMatchChar && x != InWordChar && x != NoMatchChar))
        {
            return GuessingResult.InvalidCharacter;
        }

        // User won the game, yay
        if (guessingResult.All(x => x == PositionalMatchChar))
        {
            GetGuessCount++;
            return GuessingResult.Win;
        }

        return ProcessGuessingResult(guessingResult, guess);
    }

    private string GetNoScoreCharacters()
    {
        string noScore = string.Empty;
        noScore = _guessingResultsDictionary
            .Where(x => x.GuessingResultCharacter == PositionalMatchChar)
            .Select(x => x.Character)
            .Aggregate(noScore, (current, next) => current + next);

        return noScore;
    }

    private GuessingResult ProcessGuessingResult(string guessingResult, string guess)
    {
        GetGuessCount++;

        for (int i = 0; i < guessingResult.Length; i++)
        {
            char currentChar = guess[i];
            char charResult = guessingResult[i];

            if (!_matchingChars.Contains(charResult))
            {
                throw new ArgumentException("Invalid character in guess");
            }

            if (charResult != NoMatchChar && _guessingResultsDictionary.Any(x =>
                    x.Character == charResult && x.GuessingResultCharacter == NoMatchChar))
            {
                InconsistentGuessingResultMessage =
                    $"Letter '{charResult}' at position {i} was marked as not in word before.";
                return GuessingResult.InconsistentGuessingResults;
            }

            if (charResult == NoMatchChar && _guessingResultsDictionary.Any(x =>
                    x.Character == charResult && x.GuessingResultCharacter != NoMatchChar))
            {
                InconsistentGuessingResultMessage =
                    $"Letter '{charResult}' at position {i} was marked as in word before.";
                return GuessingResult.InconsistentGuessingResults;
            }

            if (charResult != PositionalMatchChar && _guessingResultsDictionary.Any(x =>
                    x.Position == i && x.Character != charResult && x.GuessingResultCharacter == PositionalMatchChar))
            {
                InconsistentGuessingResultMessage =
                    $"Another character as the current one '{charResult}' was marked as correct at position {i} before.";
                return GuessingResult.InconsistentGuessingResults;
            }

            AddGuessingResult(i, currentChar, charResult);
        }

        _currentGuess = null;
        return GuessingResult.Processed;
    }

    public void AddGuessingResult(int position, char character, char result)
    {
        if (position > _wordLength)
        {
            throw new ArgumentException("Position is out of bounds");
        }

        _guessingResultsDictionary.Add(new GuessedCharacter
        {
            Position = position, Character = character, GuessingResultCharacter = result
        });
    }
}