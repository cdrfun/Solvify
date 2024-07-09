using Solvify.Enum;
using System;
using System.Collections.Generic;

namespace Solvify.Cli.Records;

public record GameSettings
{
    public required string Name { get; init; }
    public Uri? WebisteUrl { get; set; }
    public required PuzzleType PuzzleType { get; init; }
    public required Language Language { get; init; }
    public List<string> InvalidWords { get; } = new();
    public required string ValidCharacters { get; init; }
    public required int MaximumTries { get; init; }
    public required int GuessLength { get; init; }
}
