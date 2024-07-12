using Solvify.Cli.Enums;
using System;
using System.Collections.Generic;

namespace Solvify.Cli.Records;

public record GameSetting
{
    public required string Name { get; init; }
    public Uri? WebsiteUrl { get; set; }
    public required PuzzleType PuzzleType { get; init; }
    public required Language Language { get; init; }
    public List<string> InvalidWords { get; } = [];
    public required string ValidCharacters { get; init; }
    public required int MaximumTries { get; init; }
    public required int GuessLength { get; init; }
    public Dictionary<string, string> SubstitutionList { get; init; } = [];

    public override string ToString()
    {
        return Name;
    }
}
