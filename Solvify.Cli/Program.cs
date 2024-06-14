using System;
using System.Collections.Generic;
using System.IO;
using Solvify.Cli.Records;
using Solvify.Cli.Services;

namespace Solvify.Cli;

public class Program
{
    public static void Main()
    {
        const int maximumCharacters = 5;

        List<string> wordList = ReadWordlistFile("wordlist.txt");

        DeductWordService solver = new DeductWordService(maximumCharacters, wordList);

        while (solver.GetLastGuessingResult != DeductWordService.GuessingResult.Win)
        {
            ScoredWord guess = solver.GetCurrentGuess();
            Console.WriteLine($"Next guess: {guess.Word} with a score of {guess.Score}");
            _ = solver.AddCurrentGuessResult(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(solver.GetLastGuessingResultMessage());
        }
    }

    private static List<string> ReadWordlistFile(string filename)
    {
        List<string> wordList = [];
        using StreamReader reader = new StreamReader(filename);

        while (!reader.EndOfStream)
            wordList.Add(reader.ReadLine() ?? throw new ArgumentException());

        return wordList;
    }
}