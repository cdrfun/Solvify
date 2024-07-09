namespace Solvify.Cli.Records;
public record SolvifySettings
{
    public char InvalidWordChar = 'x';
    public char InWordChar = '+';
    public char NoMatchChar = '-';
    public char PositionalMatchChar = '*';
}
 