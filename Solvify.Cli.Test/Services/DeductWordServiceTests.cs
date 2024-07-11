using Solvify.Cli.Enums;
using Solvify.Cli.Records;
using Solvify.Cli.Services;


namespace Solvify.Cli.Test.Services;

[TestClass]
public class DeductWordServiceTests
{
    private readonly List<string> _simpleWordlist = ["apple", "banana", "cherry"];
    private readonly List<string> _simpleWordlistUpperCase = ["Apple", "Banana", "Cherry"];

    private readonly SolvifySetting _defaultSetting = new();

    //#region Debug Game

    //private record SimulatedGame
    //{
    //    public required List<string> Words { get; init; }
    //    public required List<SimulatedGuesses> Guesses { get; init; }
    //}

    //private record SimulatedGuesses
    //{
    //    public required string ExpectedGuess { get; init; }
    //    public required string GuessingResult { get; init; }
    //}

    //private readonly List<string> wordsfromFile = [];

    //[TestInitialize]
    //public void TestInitialize()
    //{
    //    using StreamReader reader = new("wordlist.txt");

    //    while (!reader.EndOfStream)
    //    {
    //        wordsfromFile.Add(reader.ReadLine() ?? throw new ArgumentException());
    //    }
    //}

    //[TestMethod]
    //public void GetCurrentGuess_DebugGame()
    //{
    //    // Arrange
    //    SimulatedGame game = new()
    //    {
    //        Words = ["raste", "holen", "fidel", "kegel", "kübel"],
    //        Guesses =
    //        [
    //            new(){ExpectedGuess = "Aster", GuessingResult = "*-+--"},
    //            new(){ExpectedGuess = "Anmut", GuessingResult = "*--+*"},
    //            new(){ExpectedGuess = "Adult", GuessingResult = "*****"}
    //        ]
    //    };
    //    string validCharacters = "abcdefghijklmnopqrstuvwxyzüöäß"; // wördle

    //    DeductWordService service = new(5, wordsfromFile, validCharacters);

    //    // Act and Assert - yeah, I know, this is a bit of a stretch
    //    foreach (SimulatedGuesses guess in game.Guesses)
    //    {
    //        var currentGuess = service.GetCurrentGuess();
    //        if (guess.ExpectedGuess.ToLower() != currentGuess.Word.ToLower())
    //        {
    //            Assert.Fail($"Expected guess {guess.ExpectedGuess} does not match current guess {currentGuess.Word}");
    //        }
    //        service.AddCurrentGuessResult(guess.GuessingResult);
    //    }
    //}

    //#endregion Debug Game

    #region AddCurrentGuessResult

    [TestMethod]
    public void AddCurrentGuessResult_InvalidWord_ReturnsInvalidWord()
    {
        // Arrange
        //DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);


        // Act
        GuessingResult result = service.AddCurrentGuessResult("x");

        // Assert
        Assert.AreEqual(GuessingResult.InvalidWord, result);
    }

    [TestMethod]
    public void AddCurrentGuessResult_InvalidLength_ReturnsInvalidLength()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);

        // Act
        GuessingResult result = service.AddCurrentGuessResult("appleee");

        // Assert
        Assert.AreEqual(GuessingResult.InvalidLength, result);
    }

    [TestMethod]
    public void AddCurrentGuessResult_InvalidCharacter_ReturnsInvalidCharacter()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);

        // Act
        GuessingResult result = service.AddCurrentGuessResult("ap+le");

        // Assert
        Assert.AreEqual(GuessingResult.InvalidCharacter, result);
    }

    [TestMethod]
    public void AddCurrentGuessResult_ValidGuess_ReturnsProcessed()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);

        // Act
        GuessingResult result = service.AddCurrentGuessResult("-----");

        // Assert
        Assert.AreEqual(GuessingResult.Processed, result);
    }

    #endregion AddCurrentGuessResult

    #region GetCurrentGuess

    [TestMethod]
    public void GetCurrentGuess_ReturnsScoredWord()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);

        // Act
        ScoredWord guess = service.GetCurrentGuess();

        // Assert
        Assert.IsNotNull(guess);
        Assert.IsInstanceOfType(guess, typeof(ScoredWord));
    }

    [TestMethod]
    public void GetCurrentGuess_UseUpperCaseWords()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlistUpperCase);

        // Act
        ScoredWord guess = service.GetCurrentGuess();

        // Assert
        Assert.IsNotNull(guess);
        Assert.IsInstanceOfType(guess, typeof(ScoredWord));
        Assert.IsFalse(guess.Word == string.Empty);
    }

    #endregion GetCurrentGuess

    #region GetLastGuessingResultMessage

    [TestMethod]
    public void GetLastGuessingResultMessage_Win_ReturnsWinMessage()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        _ = service.AddCurrentGuessResult("*****");

        // Act
        string message = service.GetLastGuessingResultMessage();

        // Assert
        Assert.AreEqual("You won! It took us 1 guesses to solve the puzzle", message);
    }

    [TestMethod]
    public void GetLastGuessingResultMessage_Processed_ReturnsProcessedMessage()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        _ = service.AddCurrentGuessResult("-----");

        // Act
        string message = service.GetLastGuessingResultMessage();

        // Assert
        Assert.AreEqual("Processed guess 1", message);
    }

    [TestMethod]
    public void GetLastGuessingResultMessage_InvalidCharacter_ReturnsInvalidCharacterMessage()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        _ = service.AddCurrentGuessResult("ap+le");

        // Act
        string message = service.GetLastGuessingResultMessage();

        // Assert
        Assert.AreEqual("Invalid character entered in result", message);
    }

    [TestMethod]
    public void GetLastGuessingResultMessage_InvalidLength_ReturnsInvalidLengthMessage()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        _ = service.AddCurrentGuessResult("appleee");

        // Act
        string message = service.GetLastGuessingResultMessage();

        // Assert
        Assert.AreEqual("Result must match length of guessed word", message);
    }

    [TestMethod]
    public void GetLastGuessingResultMessage_InvalidWord_ReturnsInvalidWordMessage()
    {
        // Arrange
        DeductWordService service = new(_defaultSetting, _defaultSetting.GameSettings.Single(x => x.Name == "6mal5"), _simpleWordlist);
        _ = service.AddCurrentGuessResult("x");

        // Act
        string message = service.GetLastGuessingResultMessage();

        // Assert
        Assert.AreEqual("Damn!", message);
    }

    #endregion GetLastGuessingResultMessage
}