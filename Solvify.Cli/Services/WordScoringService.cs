using Solvify.Cli.Records;
using Solvify.Cli.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Solvify.Cli.Services;

/// <summary>
///     Represents a class for scoring characters based on their frequency in a collection of words the class was
///     initialized with.
/// </summary>
public class WordScoringService : IWordScoringService
{
    private readonly Dictionary<char, ScoredCharacter> _characters = [];
    private readonly string _scoredCharacters;

    public WordScoringService(IEnumerable<string> words, string scoredCharacters)
    {
        _scoredCharacters = scoredCharacters;

        foreach (string word in words)
        {
            foreach (char c in word.ToLower())
            {
                if (_characters.TryGetValue(c, out ScoredCharacter? character))
                {
                    character.Count++;
                }
                else
                {
                    _characters.Add(c,
                        new ScoredCharacter { Character = c, Count = 0 });
                }
            }
        }

        List<ScoredCharacter> orderedCharacters = _characters.Values.OrderBy(x => x.Count)
            .ToList();

        for (int i = 0; i < orderedCharacters.Count; i++)
        {
            orderedCharacters[i].Score = i + 1;
        }
    }

    /// <summary>
    ///     Calculates the score for a given word based on the frequency of its characters.
    /// </summary>
    /// <param name="word">The word to calculate the score for.</param>
    /// <param name="noScoreCharacters">The characters that should not be included in the score calculation.</param>
    /// <returns>The score of the word, based on the words the CharacterScoring was initialised with</returns>
    public int GetScore(string word, string noScoreCharacters)
    {
        return word.ToLower()
            .All(x => _scoredCharacters.ToLower()
                .Contains(x))
            ? word.ToLower()
                .Where(x => !noScoreCharacters.Contains(x))
                .Distinct()
                .Sum(x => _characters[x].Score)
            : 0;
    }
}