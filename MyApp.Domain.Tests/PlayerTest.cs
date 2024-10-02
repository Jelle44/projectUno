using Xunit;

namespace MyApp.Domain.Tests;
public class PlayerTest
{
    [Fact]
    public void TestPlayerExists()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        var playerTwo = new Player(names);

        //Assert
        Assert.NotNull(playerTwo);
    }

    [Fact]
    public void TestPlayerHasNextPlayer()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.NotNull(playerTwo.NextPlayer);
    }

    [Fact]
    public void TestPlayerHasPreviousPlayer()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.NotNull(playerTwo.PreviousPlayer);
    }

    [Fact]
    public void TestThreePlayersKnowEachOther()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        const string playerThreeName = "Kimmy";
        string[] names = { playerOneName, playerTwoName, playerThreeName };

        //Act
        Player playerThree = new (names);

        //Assert
        Assert.Equal(playerThreeName, playerThree.Name);
        Assert.Equal(playerTwoName, playerThree.NextPlayer.Name);
        Assert.Equal(playerOneName, playerThree.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestThreePlayersPlayerOneAndFourAreSame()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        const string playerThreeName = "Kimmy";
        string[] names = { playerOneName, playerTwoName, playerThreeName };

        //Act
        Player playerThree = new(names);

        //Assert
        Assert.Equal(playerThreeName, playerThree.NextPlayer.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestPlayerHasUnoProperty()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.False(playerTwo.Uno);
    }

    [Fact]
    public void TestPlayerHasName()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.Equal(playerTwoName, playerTwo.Name);
    }

    [Fact]
    public void TestPlayersAllHaveCorrectPrevPlayer()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.Equal(playerOneName, playerTwo.PreviousPlayer.Name);
        Assert.Equal(playerTwoName, playerTwo.NextPlayer.PreviousPlayer.Name);
    }

    [Fact]
    public void TestDrawCardSwitchesTurn()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] names = { playerOneName, playerTwoName };
        Game game = new(names);

        //Act
        game.Deck.DrawCard(playerOneName);

        //Assert
        Assert.Equal(playerOneName, game.Deck.Pile.Owner!.Name);
    }
}
