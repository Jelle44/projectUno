using Moq;
using MyApp.Domain.Exceptions;
using MyApp.Domain.Factories;
using Xunit;
using static MyApp.Domain.Card;
using static MyApp.Domain.CardSuperClass;

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
        Assert.NotNull( pile.Owner);
    }

    [Fact]
    public void TestPileHasActiveColour()
    {
        //Arrange
        string[] players = { "Timmy" };

        //Act
        Pile pile = new(players);

        //Assert
        Assert.Equal(Colour.ALL, pile.ActiveColour);
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
        Card card = new(pile) {
            Owner = pile.Owner,
            ActiveColour = Colour.BLUE,
            ActiveValue = Value.FOUR
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
        Card card = new(pile)
        {
            Owner = pile.Owner,
            ActiveValue = Value.ZERO,
            ActiveColour = Colour.BLUE
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
        string[] players = { "Timmy", "Jimmy" };
        Deck deck = new(players);
        Pile pile = new(players);
        Card card = new(pile)
        {
            Owner = pile.Owner!.GetPlayerByName("Timmy"),
            ActiveValue = Value.ZERO,
            ActiveColour = Colour.BLUE
        };

        pile.ActiveValue = Value.ONE;
        pile.ActiveColour = Colour.RED;
        pile.Owner = pile.Owner.GetPlayerByName("Jimmy");

        Card[] playerHand = { card };

        //Act / Assert
        Assert.Throws<InvalidCardException>(() => PlayCard(deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestReverseTurnReversesTurn()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy", "Barney" };
        Deck deck = new(players);
        Pile pile = new(players);
        Card cardJimmy = new(pile)
        {
            Owner = pile.Owner!.GetPlayerByName("Jimmy"),
            ActiveColour = Colour.RED,
            ActiveValue = Value.REVERSE
        };
        Card cardBarney = new(pile)
        {
            Owner = pile.Owner!.GetPlayerByName("Barney"),
            ActiveColour = Colour.RED
        };

        Card[] playerOneHand = { cardJimmy };
        PlayCard(deck, playerOneHand, cardJimmy.ActiveValue, cardJimmy.ActiveColour, cardJimmy.ActiveColour);

        Assert.Equal("Jimmy", pile.Owner.Name);
        Assert.Equal("Timmy", pile.Owner.NextPlayer.Name);

        Card[] playerTwoHand = { cardBarney };

        //Act
        PlayCard(deck, playerTwoHand, cardBarney.ActiveValue, cardBarney.ActiveColour, cardBarney.ActiveColour);

        //Assert
        Assert.Equal("Barney", pile.Owner.Name);
    }

    [Fact]
    public void TestSkipTurnSkipsTurn()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy", "Barney" };
        Deck deck = new(players);
        Pile pile = new(players);
        Card cardJimmy = new(pile)
        {
            Owner = pile.Owner!.GetPlayerByName("Jimmy"),
            ActiveColour = Colour.RED,
            ActiveValue = Value.SKIPTURN
        };
        Card cardBarney = new(pile)
        {
            Owner = pile.Owner!.GetPlayerByName("Barney"),
            ActiveColour = Colour.RED
        };

        Card[] playerOneHand = { cardJimmy };
        PlayCard(deck, playerOneHand, cardJimmy.ActiveValue, cardJimmy.ActiveColour, cardJimmy.ActiveColour);
        
        Card[] playerTwoHand = { cardBarney };

        //Act
        PlayCard(deck, playerTwoHand, cardBarney.ActiveValue, cardBarney.ActiveColour, cardBarney.ActiveColour);

        //Assert
        Assert.Equal("Barney", pile.Owner.Name);
    }

    [Fact]
    public void TestRecolourChangesColour()
    {
        //Arrange
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card cardTimmy = new(pile)
        {
            Owner = pile.Owner,
            ActiveValue = Value.RECOLOUR,
            ActiveColour = Colour.ALL
        };

        Card[] playerTwoHand = { cardTimmy };
        Deck deck = new(players);

        PlayCard(deck, playerTwoHand, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);

        //Act
        pile.ChangeColour(Colour.RED);

        //Assert
        Assert.Equal(Colour.RED, pile.ActiveColour);
    }

    [Fact]
    public void TestPlayerCannotDrawTwoCardsInOneTurn()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new (players);

        game.DrawCard("Timmy");

        //Act / Assert
        Assert.Throws<NotYourTurnException>(() => game.DrawCard("Timmy"));
    }

    [Fact]
    public void TestDrawTwoDrawsTwo()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck game = new (players, "test");
        game.DrawCard("Timmy");
        Card cardTimmy = game.DrawCard("Timmy");

        cardTimmy.ActiveColour = Colour.BLUE;
        cardTimmy.ActiveValue = Value.DRAW_TWO;

        game.Pile.ActiveColour = Colour.BLUE;

        //Act
        game.UpdateGameState("Timmy", cardTimmy.ActiveValue, cardTimmy.ActiveColour, Colour.BLUE);

        //Assert
        Card[] handTimmy = game.Cards.Where(card =>
                                        card.Owner?.Name == "Timmy" &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(3, handTimmy.Length);
    }

    [Fact]
    public void TestDrawFourDrawsFour()
    {
        //Arrange
        string[] players = { "Timmy" };
        Deck game = new(players, "test");
        game.DrawCard("Timmy");
        Card cardTimmy = game.DrawCard("Timmy");

        cardTimmy.ActiveColour = Colour.ALL;
        cardTimmy.ActiveValue = Value.DRAW_FOUR;

        game.Pile.ActiveColour = Colour.BLUE;

        //Act
        game.UpdateGameState("Timmy", cardTimmy.ActiveValue, cardTimmy.ActiveColour, Colour.BLUE);

        //Assert
        Card[] handTimmy = game.Cards.Where(card =>
                                        card.Owner?.Name == "Timmy" &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(5, handTimmy.Length);
    }
}
