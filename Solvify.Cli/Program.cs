using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solvify.Cli
{
    public class Program
    {
        public static void Main()
        {
            const char positionalMatchChar = '*';
            const char inWordChar = '+';
            const char noMatchChar = '-';
            const char invalidWordChar = 'x';
            const int maximumCharacters = 5;
            const string invalidCharacters = "öäüß";

            List<string> wordList = [];

            using var reader = new StreamReader("wordlist.txt");
            while (!reader.EndOfStream)
            {
                wordList.Add(reader.ReadLine() ?? throw new ArgumentException());
            }

            List<string> activeWords = wordList.Where(x => x.Length == maximumCharacters).ToList();
            CharacterDistribution dist = new(activeWords);

            string noMatch = invalidCharacters;
            Dictionary<int, char> positionalMatches = [];
            Dictionary<int, char> inWord = [];
            while (true)
            {
                string noScore = noMatch; // maybe remove characters that are in inWord from score

                foreach (KeyValuePair<int, char> positionalMatch in positionalMatches)
                {
                    noScore += positionalMatch.Value;
                }

                noScore = noScore.ToLower();

                List<ScoredWord> scoredWordList = [];
                scoredWordList.AddRange(activeWords.Select(word => new ScoredWord()
                    { Word = word, Score = dist.GetScore(word, noScore) }));

                string guessingResult = string.Empty;
                bool validGuessingResult = false;
                ScoredWord nextGuess = new() { Word = string.Empty };
                while (!validGuessingResult)
                {
                    nextGuess = scoredWordList.OrderByDescending(x => x.Score).First();
                    Console.WriteLine($"Next guess: {nextGuess.Word} with a score of {nextGuess.Score}");
                    Console.WriteLine(
                        $"Please input Result: {positionalMatchChar} = positional match, {inWordChar} = in word, {noMatchChar} = no match");

                    guessingResult = Console.ReadLine() ?? string.Empty;

                    if (guessingResult == invalidWordChar.ToString())
                    {
                        Console.WriteLine("Damn!");
                        scoredWordList.Remove(nextGuess);
                        continue;
                    }

                    if (guessingResult.Length != nextGuess.Word.Length)
                    {
                        Console.WriteLine("Result must match length of guessed word");
                        continue;
                    }

                    if (guessingResult.Any(x => x != positionalMatchChar && x != inWordChar && x != noMatchChar))
                    {
                        Console.WriteLine("Invalid character in result");
                        continue;
                    }

                    validGuessingResult = true;
                }

                if (guessingResult.All(x => x == positionalMatchChar))
                {
                    Console.WriteLine("You won!");
                    return;
                }

                for (var i = 0; i < guessingResult.Length; i++)
                {
                    switch (guessingResult[i])
                    {
                        case positionalMatchChar:
                            positionalMatches.Add(i, nextGuess.Word[i]);
                            break;
                        case inWordChar:
                            inWord.Add(i, nextGuess.Word[i]);
                            break;
                        case noMatchChar:
                            noMatch += nextGuess.Word[i];
                            break;
                    }
                }

                string regex = "^";
                foreach (var lookaheadChar in inWord)
                {
                    regex += $"(?=.*{lookaheadChar.Value})";
                }

                for (int i = 0; i < maximumCharacters; i++)
                {
                    if (positionalMatches.TryGetValue(i, out char posMatch))
                        regex += posMatch;

                    else
                    {
                        regex += $"[^{noMatch}{noMatch.ToUpper()}";
                        if (inWord.TryGetValue(i, out char notThere))
                            regex += $"{notThere.ToString().ToUpper()}{notThere}";

                        regex += "]";
                    }
                }

                regex += "$";

                activeWords = activeWords.Where(x => Regex.IsMatch(x, regex)).ToList();
            }
        }
    }

    public class CharacterDistribution
    {
        // ReSharper disable once StringLiteralTypo
        private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyz";

        private readonly Dictionary<char, ScoredCharacter> _characters = [];

        public CharacterDistribution(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                foreach (var c in word.ToLower())
                {
                    if (_characters.TryGetValue(c, out var character))
                    {
                        character.Count++;
                    }
                    else
                    {
                        _characters.Add(c, new ScoredCharacter() { Character = c, Count = 0 });
                    }
                }
            }

            var orderedCharacters = _characters.Values.OrderBy(x => x.Count).ToList();
            for (var i = 0; i < orderedCharacters.Count; i++)
            {
                orderedCharacters[i].Score = i + 1;
            }
        }

        public int GetScore(string word, string noScoreCharacters)
        {
            if (word.ToLower().All(x => ValidCharacters.ToLower().Contains(x)))
            {
                return word.ToLower().Where(x => !noScoreCharacters.Contains(x)).Distinct()
                    .Sum(x => _characters[x].Score);
            }

            return 0;
        }
    }

    public record ScoredWord
    {
        public required string Word { get; init; }
        public int Score { get; set; }
    }

    public record ScoredCharacter
    {
        public required char Character { get; init; }
        public int Score { get; set; }
        public int Count { get; set; }
    }
}