namespace Solvify.Cli.Services.Interfaces;

/// <summary>
///     Represents a class for scoring characters based on their frequency in a collection of words the class was
///     initialized with.
/// </summary>
public interface IWordScoringService
{
    /// <summary>
    ///     Calculates the score for a given word based on the frequency of its characters.
    /// </summary>
    /// <param name="word">The word to calculate the score for.</param>
    /// <param name="noScoreCharacters">The characters that should not be included in the score calculation.</param>
    /// <returns>The score of the word, based on the words the CharacterScoring was initialised with</returns>
    public int GetScore(string word, string noScoreCharacters);
}