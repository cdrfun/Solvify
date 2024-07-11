using Solvify.Cli.Enums;
using Solvify.Cli.Records;
using Solvify.Cli.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solvify.Cli;

public class Program
{
    public static void Main()
    {
        MenuLoop(ReadWordlistFile("wordlist.txt"));
    }

    private static void MenuLoop(List<string> wordList)
    {
        SolvifySetting defaultSetting = new();
        string selected = string.Empty;

        List<string> choices = defaultSetting.GameSettings.Select(x => x.Name).ToList();
        choices.Add("Exit");

        while (selected != "Exit")
        {
            selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select Game Setting")
                    .PageSize(10)
                    .AddChoices(choices)
            );

            if (selected == "Exit")
            {
                continue;
            }

            GameSetting? selectedGame = defaultSetting.GameSettings.SingleOrDefault(x => x.Name == selected);
            if (selectedGame != null)
            {
                DeductWordService solver = new(defaultSetting, selectedGame, wordList);
                GameLoop(solver);
            }
            else
            {
                Console.WriteLine("Invalid game setting selected.");
            }
        }
    }

    private static void GameLoop(DeductWordService solver)
    {
        while (solver.GetLastGuessingResult != GuessingResult.Win)
        {
            ScoredWord guess = solver.GetCurrentGuess();
            Console.WriteLine(
                $"Next guess: {guess.Word} with a score of {guess.Score}. {solver.ActiveWordsOfLastGuess} words are still active.");
            GuessingResult result = solver.AddCurrentGuessResult(Console.ReadLine() ?? string.Empty);
            Console.WriteLine(result == GuessingResult.InconsistentGuessingResults
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