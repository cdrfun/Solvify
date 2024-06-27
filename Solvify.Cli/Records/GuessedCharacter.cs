namespace Solvify.Cli.Records;

public record GuessedCharacter
{
    public required char Character { get; init; }
    public int Position { get; set; }
    public char GuessingResultCharacter { get; set; }
}