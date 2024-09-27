using Moq;
using MyApp.Domain.Factories;
using Xunit;
using static MyApp.Domain.Card;
using static MyApp.Domain.CardSuperClass;

namespace MyApp.Domain.Tests;

public class DeckTest
{
    [Fact]
    public void TestUno()
    {
        //Arrange
        string[] playerOne = { "Timmy" };

        //Act
        Deck uno = new(playerOne);

        //Assert
        Assert.Equal(100, uno.Counter);
    }

    [Fact]
    public void TestUnoCounter()
    {
        Assert.Equal(107, Deck.DecreaseCounter(108));
    }

    [Fact]
    public void TestDrawCardCreatesCard()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne);

        //Act
        var hand = game.DrawCard("Timmy");

        //Assert
        Assert.True(hand.GetType() == typeof(Card));
    }

    [Fact]
    public void TestValidUnoUpdatesPlayerBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne, "testConstructor");

        //Act
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.True(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestInvalidUnoDoesNotUpdateBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne, "Test constructor");

        //Act
        game.DrawCard("Timmy");
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.False(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestDrawCardUpdatesPlayerBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne, "Test constructor");

        //Act
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");
        game.DrawCard("Timmy");

        //Assert
        Assert.False(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestFalseUnoDrawsCards()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne);

        //Act
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Card[] actual = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(9, actual.Length);
    }

    [Fact]
    public void TestPlayingSkipAsSecondToLastCardResultsInValidUno()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new(players, "testConstructor");

        //Act
        Card cardTimmy = game.DrawCard("Timmy");
        cardTimmy.ActiveValue = Value.SKIPTURN;
        game.DrawCard("Jimmy");

        Card otherCard = game.DrawCard("Timmy");
        game.DrawCard("Jimmy");

        Card[] playerHand = { cardTimmy, otherCard };
        PlayCard(game, playerHand, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.True(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestDeckReshufflesWhenEmpty()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck game = new(players, "testConstructor");

        game.DrawMultipleCards("Timmy", 19);

        Card[] playerHand = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                      .ToArray();

        //Act
        PlayCard(game, playerHand, Value.ZERO, Colour.RED, Colour.RED);
        game.DrawCard("Timmy");

        //Assert
        playerHand = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                               .ToArray();

        Assert.Equal(19, playerHand.Length);
    }

    [Fact]
    public void TestGameStartsWithSevenCardsInEachHand()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck game = new(players);

        //Act
        Card[] handTimmy = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        //Assert
        Assert.Equal(7, handTimmy.Length);
    }

    [Fact]
    public void TestGameEndsWhenPlayerHasNoCardsLeft()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new(players, "Test Constructor");
        game.Pile.Owner!.Uno = true;
        Card card = game.DrawCard("Timmy");
        game.Pile.Owner.PreviousPlayer.Uno = true;

        //Act
        game.DrawCard("Jimmy");

        //Assert
        Assert.NotNull(game.UpdateGameState("Timmy", card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestGetPlayerNamesListReturnsListOfAllPlayerNames()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy", "Barney" };
        Deck game = new(players);

        //Act
        var list = game.CreatePlayerList("Timmy");

        //Assert
        Assert.Equal(3, list.Length);
    }
}
