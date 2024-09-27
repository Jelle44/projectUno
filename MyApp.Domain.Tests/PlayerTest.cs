using Moq;
using MyApp.Domain.Factories;
using Xunit;

namespace MyApp.Domain.Tests;
public class PlayerTest
{
    [Fact]
    public void TestPlayerExists()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        var playerTwo = new Player(names);

        //Assert
        Assert.NotNull(playerTwo);
    }

    [Fact]
    public void TestPlayerHasNextPlayer()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.NotNull(playerTwo.NextPlayer);
    }

    [Fact]
    public void TestPlayerHasPreviousPlayer()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.NotNull(playerTwo.PreviousPlayer);
    }

    [Fact]
    public void TestThreePlayersKnowEachOther()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy", "Kimmy" };

        //Act
        Player playerThree = new (names);

        //Assert
        Assert.Equal("Kimmy", playerThree.Name);
        Assert.Equal("Jimmy", playerThree.NextPlayer.Name);
        Assert.Equal("Timmy", playerThree.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestThreePlayersPlayerOneAndFourAreSame()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy", "Kimmy" };

        //Act
        Player playerThree = new(names);

        //Assert
        Assert.Equal("Kimmy", playerThree.NextPlayer.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestPlayerHasUnoProperty()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.False(playerTwo.Uno);
    }

    [Fact]
    public void TestPlayerHasName()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.Equal("Jimmy", playerTwo.Name);
    }

    [Fact]
    public void TestPlayersAllHaveCorrectPrevPlayer()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };

        //Act
        Player playerTwo = new(names);

        //Assert
        Assert.Equal("Timmy", playerTwo.PreviousPlayer.Name);
        Assert.Equal("Jimmy", playerTwo.NextPlayer.PreviousPlayer.Name);
    }

    [Fact]
    public void TestDrawCardSwitchesTurn()
    {
        //Arrange
        string[] names = { "Timmy", "Jimmy" };
        var cardFactory = new Mock<ICardFactory>();
        Deck game = new(names, cardFactory.Object);

        //Act
        game.DrawCard("Timmy");

        //Assert
        Assert.Equal("Timmy", game.Pile.Owner!.Name);
    }
}
