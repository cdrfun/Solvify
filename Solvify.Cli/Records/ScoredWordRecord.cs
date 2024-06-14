namespace Solvify.Cli.Records;

public record ScoredWord
{
    public required string Word { get; init; }
    public int Score { get; set; }
}