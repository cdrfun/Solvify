namespace Solvify.Cli.Records;

public record ScoredCharacter
{
    public required char Character { get; init; }
    public int Score { get; set; }
    public int Count { get; set; }
}