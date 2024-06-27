using Solvify.Cli.Records;
using Solvify.Cli.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Solvify.Cli;

public class Program
{
    public static void Main()
    {
        const int maximumCharacters = 5;

        List<string> wordList = ReadWordlistFile("wordlist.txt");

        string validCharacters = "abcdefghijklmnopqrstuvwxyz"; // 6mal5
        //string validCharacters = "abcdefghijklmnopqrstuvwxyzüöäß"; // wördle
        DeductWordService solver = new(maximumCharacters, wordList, validCharacters);

        while (solver.GetLastGuessingResult != DeductWordService.GuessingResult.Win)
        {
            ScoredWord guess = solver.GetCurrentGuess();
            Console.WriteLine($"Next guess: {guess.Word} with a score of {guess.Score}. {solver.ActiveWordsOfLastGuess} words are still active.");
            DeductWordService.GuessingResult result = solver.AddCurrentGuessResult(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(result == DeductWordService.GuessingResult.InconsistentGuessingResults
                ? solver.InconsistentGuessingResultMessage
                : solver.GetLastGuessingResultMessage());
        }
    }

    private static List<string> ReadWordlistFile(string filename)
    {
        List<string> wordList = [];
        using StreamReader reader = new(filename);

        while (!reader.EndOfStream)
        {
            wordList.Add(reader.ReadLine() ?? throw new ArgumentException());
        }

        return wordList;
    }
}