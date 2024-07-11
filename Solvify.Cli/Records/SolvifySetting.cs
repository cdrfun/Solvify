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
            PuzzleType = PuzzleType.DeductWord,
            Language = Language.German,
            ValidCharacters = "abcdefghijklmnopqrstuvwxyz",
            MaximumTries = 6,
            GuessLength = 5
        },

        new GameSetting
        {
            Name = "wördle",
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
