using Solvify.Cli.Enums;
using System.Collections.Generic;

namespace Solvify.Cli.Records;

public record SolvifySetting
{
    public List<GameSetting> GameSettings =
    [
        new GameSetting
        {
            Name = "6mal5",
            WebsiteUrl = new("https://6mal5.com/"),
            PuzzleType = PuzzleType.DeductWord,
            Language = Language.German,
            ValidCharacters = "abcdefghijklmnopqrstuvwxyz",
            MaximumTries = 6,
            GuessLength = 5,
            SubstitutionList = new Dictionary<string, string>
            {
                {"ä", "ae"},
                {"ö", "oe"},
                {"ü", "ue"},
                {"Ä", "Ae"},
                {"Ö", "Oe"},
                {"Ü", "Ue"}
            }
        },

        new GameSetting
        {
            Name = "wördle",
            WebsiteUrl = new("https://www.wördle.de/"),
            PuzzleType = PuzzleType.DeductWord,
            Language = Language.German,
            ValidCharacters = "abcdefghijklmnopqrstuvwxyzüöäß",
            MaximumTries = 6,
            GuessLength = 5
        }
    ];

    public char InvalidWordChar = 'x';
    public char InWordChar = '+';
    public char NoMatchChar = '-';
    public char PositionalMatchChar = '*';
}
