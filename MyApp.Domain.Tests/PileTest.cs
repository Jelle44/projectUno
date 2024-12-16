using MyApp.Domain.Enums;
using MyApp.Domain.Exceptions;
using Xunit;
using static MyApp.Domain.Card;

namespace MyApp.Domain.Tests;
public class PileTest
{
    [Fact]
    public void TestPileExists()
    {
        //Arrange
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.NotNull(pile);
    }

    [Fact]
    public void TestPileHasOwner()
    {
        //Arrange
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.NotNull(pile.Owner);
    }

    [Fact]
    public void TestPileHasActiveColour()
    {
        //Arrange
        const Colour expectedStartingValueOfPile = Colour.WILD;
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.Equal(expectedStartingValueOfPile, pile.ActiveColour);
    }

    [Fact]
    public void TestPileHasActiveValue()
    {
        //Arrange
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.Equal(Value.FOUR, pile.ActiveValue);
    }

    [Fact]
    public void TestPileHasTurnOrder()
    {
        //Arrange
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.False(pile.TurnOrderIsReversed);
    }

    [Fact]
    public void TestPileAllowsSameColour()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck deck = new(players);
        Pile pile = new(players)
        { 
            ActiveColour = Colour.BLUE,
            ActiveValue = Value.ZERO
        };
        Card card = new(pile, Colour.BLUE, Value.FOUR) {
            Owner = pile.Owner
        };

        Card[] playerHand = { card };

        //Act
        PlayCard(deck, playerHand,card.ActiveValue, card.ActiveColour, card.ActiveColour);

        //Assert
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
    }

    [Fact]
    public void TestPileAllowsSameValueDifferentColour()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck deck = new(players);
        Pile pile = new(players)
        {
            ActiveColour = Colour.BLUE,
            ActiveValue = Value.ZERO
        };
        Card card = new(pile, Colour.BLUE, Value.ZERO)
        {
            Owner = pile.Owner
        };

        Card[] playerHand = { card };

        //Act
        PlayCard(deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour);

        //Assert
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPileDoesNotAllowDifferentColour()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] players = { playerOneName, playerTwoName };
        Deck deck = new(players);
        Pile pile = new(players);
        Card card = new(pile, Colour.BLUE, Value.ZERO)
        {
            Owner = pile.Owner!.GetPlayerByName(playerOneName)
        };

        pile.ActiveValue = Value.ONE;
        pile.ActiveColour = Colour.RED;
        pile.Owner = pile.Owner.GetPlayerByName(playerTwoName);

        Card[] playerHand = { card };

        //Act / Assert
        Assert.Throws<InvalidCardException>(() => PlayCard(deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestReverseTurnReversesTurn()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        const string playerThreeName = "Barney";
        string[] players = { playerOneName, playerTwoName, playerThreeName };
        Deck deck = new(players);
        Pile pile = new(players);
        Card playerTwoCard = new(pile, Colour.RED, Value.REVERSE)
        {
            Owner = pile.Owner!.GetPlayerByName(playerTwoName)
        };
        Card playerThreeCard = new(pile, Colour.RED, Value.ZERO)
        {
            Owner = pile.Owner!.GetPlayerByName(playerThreeName)
        };

        Card[] playerOneHand = { playerTwoCard };
        PlayCard(deck, playerOneHand, playerTwoCard.ActiveValue, playerTwoCard.ActiveColour, playerTwoCard.ActiveColour);

        Assert.Equal(playerTwoName, pile.Owner.Name);
        Assert.Equal(playerOneName, pile.Owner.NextPlayer.Name);

        Card[] playerTwoHand = { playerThreeCard };

        //Act
        PlayCard(deck, playerTwoHand, playerThreeCard.ActiveValue, playerThreeCard.ActiveColour, playerThreeCard.ActiveColour);

        //Assert
        Assert.Equal(playerThreeName, pile.Owner.Name);
    }

    [Fact]
    public void TestSkipTurnSkipsTurn()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        const string playerThreeName = "Barney";
        string[] players = { playerOneName, playerTwoName, playerThreeName };
        Deck deck = new(players);
        Pile pile = new(players);
        Card playerTwoCard = new(pile, Colour.RED, Value.SKIP)
        {
            Owner = pile.Owner!.GetPlayerByName(playerTwoName)
        };
        Card playerThreeCard = new(pile, Colour.RED, Value.ZERO)
        {
            Owner = pile.Owner!.GetPlayerByName(playerThreeName)
        };

        Card[] playerOneHand = { playerTwoCard };
        PlayCard(deck, playerOneHand, playerTwoCard.ActiveValue, playerTwoCard.ActiveColour, playerTwoCard.ActiveColour);
        
        Card[] playerTwoHand = { playerThreeCard };

        //Act
        PlayCard(deck, playerTwoHand, playerThreeCard.ActiveValue, playerThreeCard.ActiveColour, playerThreeCard.ActiveColour);

        //Assert
        Assert.Equal(playerThreeName, pile.Owner.Name);
    }

    [Fact]
    public void TestRecolourChangesColour()
    {
        //Arrange
        const string playerOneName = "Timmy";
        string[] players = { playerOneName };
        Pile pile = new(players);

        //Act
        pile.ChangeColour(Colour.RED);

        //Assert
        Assert.Equal(Colour.RED, pile.ActiveColour);
    }

    [Fact]
    public void TestPlayerCannotDrawTwoCardsInOneTurn()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] players = { playerOneName, playerTwoName };
        Deck game = new (players);

        game.DrawCard(playerOneName);

        //Act / Assert
        Assert.Throws<NotYourTurnException>(() => game.DrawCard(playerOneName));
    }

    [Fact]
    public void TestDrawTwoDrawsTwo()
    {
        //Arrange
        const string playerOneName = "Timmy";
        string[] players = { playerOneName };
        Deck game = new (players, "test");
        game.DrawCard(playerOneName);
        Card playerOneCard = game.DrawCard(playerOneName);

        playerOneCard.ActiveColour = Colour.BLUE;
        playerOneCard.ActiveValue = Value.DRAW2;

        game.Pile.ActiveColour = Colour.BLUE;

        //Act
        game.UpdateGameState(playerOneName, playerOneCard.ActiveValue, playerOneCard.ActiveColour, Colour.BLUE);

        //Assert
        Card[] actual = game.Cards.Where(card =>
                                        card.Owner?.Name == playerOneName &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(3, actual.Length);
    }

    [Fact]
    public void TestDrawFourDrawsFour()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] players = { playerName };
        Deck game = new(players, "test");
        game.DrawCard(playerName);
        Card playerCard = game.DrawCard(playerName);

        playerCard.ActiveColour = Colour.WILD;
        playerCard.ActiveValue = Value.DRAW4;

        game.Pile.ActiveColour = Colour.BLUE;

        //Act
        game.UpdateGameState(playerName, playerCard.ActiveValue, playerCard.ActiveColour, Colour.BLUE);

        //Assert
        Card[] actual = game.Cards.Where(card =>
                                        card.Owner?.Name == playerName &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(5, actual.Length);
    }
}
