using Solvify.Cli.Records;
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

    private readonly List<string> invalidWordList = [];
    private readonly Dictionary<int, char> inWord = [];
    private readonly Dictionary<int, char> positionalMatches = [];
    private readonly WordScoringService scoringService;
    private readonly int wordLength;
    private readonly List<string> wordlist;
    private ScoredWord? currentGuess;
    private string noMatch = string.Empty;
    private readonly string validCharacters = string.Empty;

    public DeductWordService(int wordLength, List<string> wordlist, string validCharacters)
    {
        this.validCharacters = validCharacters;
        scoringService = new WordScoringService(wordlist.Where(x => x.Length == wordLength), validCharacters);
        this.wordLength = wordLength;
        this.wordlist = wordlist;
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
        if (currentGuess is not null)
        {
            return currentGuess;
        }

        string noScore = noMatch; // maybe remove characters that are in inWord from score

        foreach (KeyValuePair<int, char> positionalMatch in positionalMatches)
        {
            noScore += positionalMatch.Value;
        }

        noScore = noScore.ToLower();

        IEnumerable<string> activeWords = wordlist.Where(x => x.Length == wordLength)
                                                  .Where(x => !invalidWordList.Contains(x))
                                                  .Where(x => x.All(c => validCharacters.Contains(c, System.StringComparison.OrdinalIgnoreCase)));

        if (GetGuessCount > 0)
        {
            string regex = CreateRegexPattern();
            activeWords = activeWords.Where(x => Regex.IsMatch(x, regex, RegexOptions.IgnoreCase));
        }

        List<ScoredWord> scoredWordList = activeWords.Select(word => new ScoredWord
        {
            Word = word,
            Score = scoringService.GetScore(word, noScore)
        }).ToList();

        List<ScoredWord> guessList = scoredWordList.OrderByDescending(x => x.Score).ThenBy(x => x.Word)
                                         .ToList();
        ScoredWord guess = guessList.FirstOrDefault() ?? new() { Score = 0, Word = string.Empty }; // I have no word, maybe throw exception?

        currentGuess = guess;
        return currentGuess;
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

    private string CreateRegexPattern()
    {
        string regex = inWord.Where(i => !positionalMatches.Any(m => m.Value == i.Value)).Aggregate("^", (current, lookaheadChar) => current + $"(?=.*{lookaheadChar.Value})");

        for (int i = 0; i < wordLength; i++)
        {
            if (positionalMatches.TryGetValue(i, out char posMatch))
            {
                regex += posMatch;
            }
            else
            {
                regex += $"[^{noMatch}";

                if (inWord.TryGetValue(i, out char notHereChar))
                {
                    regex += $"{notHereChar}";
                }

                regex += "]";
            }
        }

        regex += "$";
        return regex;
    }

    private GuessingResult GetCurrentGuessResult(string guessingResult)
    {
        string guess = currentGuess?.Word
                       ?? GetCurrentGuess()
                           .Word;

        if (guessingResult == InvalidWordChar.ToString())
        {
            invalidWordList.Add(guess);
            currentGuess = null;
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

    private void ProcessGuessingResult(string guessingResult, string guess)
    {
        GetGuessCount++;

        for (int i = 0; i < guessingResult.Length; i++)
        {
            switch (guessingResult[i])
            {
                case PositionalMatchChar:
                    _ = positionalMatches.TryAdd(i, guess[i]);
                    break;

                case InWordChar:
                    _ = inWord.TryAdd(i, guess[i]);
                    break;

                case NoMatchChar:
                    noMatch += guess[i];
                    break;
            }
        }

        currentGuess = null;
    }
}