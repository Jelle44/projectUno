using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyApp.Domain.CardSuperClass;
using static MyApp.Domain.Card;
using Xunit;
using MyApp.Domain.Exceptions;

namespace MyApp.Domain.Tests;
public class PileTest
{
    [Fact]
    public void TestPileExists()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Assert.NotNull(pile);
    }

    [Fact]
    public void TestPileHasOwner()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Assert.NotNull( pile.Owner);
    }

    [Fact]
    public void TestPileHasActiveColour()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Assert.Equal(Colour.ALL, pile.ActiveColour);
    }

    [Fact]
    public void TestPileHasActiveValue()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Assert.Equal(Value.FOUR, pile.ActiveValue);
    }

    [Fact]
    public void TestPileHasTurnOrder()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Assert.False(pile.TurnOrderIsReversed);
    }

    [Fact]
    public void TestPileAllowsSameColour()
    {
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

        PlayCard(deck, playerHand,card.ActiveValue, card.ActiveColour, card.ActiveColour);

        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
    }

    [Fact]
    public void TestPileAllowsSameValueDifferentColour()
    {
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

        PlayCard(deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour);

        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPileDoesNotAllowDifferentColour()
    {
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

        Assert.Throws<InvalidCardException>(() => PlayCard(deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestReverseTurnReversesTurn()
    {
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
        PlayCard(deck, playerTwoHand, cardBarney.ActiveValue, cardBarney.ActiveColour, cardBarney.ActiveColour);

        Assert.Equal("Barney", pile.Owner.Name);
    }

    [Fact]
    public void TestSkipTurnSkipsTurn()
    {
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
        PlayCard(deck, playerTwoHand, cardBarney.ActiveValue, cardBarney.ActiveColour, cardBarney.ActiveColour);

        Assert.Equal("Barney", pile.Owner.Name);
    }

    [Fact]
    public void TestRecolourChangesColour()
    {
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
        pile.ChangeColour(Colour.RED);

        Assert.Equal(Colour.RED, pile.ActiveColour);
    }

    [Fact]
    public void TestPlayerCannotDrawTwoCardsInOneTurn()
    {
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new (players);

        game.DrawCard("Timmy");

        Assert.Throws<NotYourTurnException>(() => game.DrawCard("Timmy"));
    }

    [Fact]
    public void TestDrawTwoDrawsTwo()
    {
        string[] players = { "Timmy" };
        Deck game = new (players, "test");
        game.DrawCard("Timmy");
        Card cardTimmy = game.DrawCard("Timmy");

        cardTimmy.ActiveColour = Colour.BLUE;
        cardTimmy.ActiveValue = Value.DRAW_TWO;

        game.Pile.ActiveColour = Colour.BLUE;

        game.UpdateGameState("Timmy", cardTimmy.ActiveValue, cardTimmy.ActiveColour, Colour.BLUE);

        Card[] handTimmy = game.Cards.Where(card =>
                                        card.Owner?.Name == "Timmy" &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(3, handTimmy.Length);
    }

    [Fact]
    public void TestDrawFourDrawsFour()
    {
        string[] players = { "Timmy" };
        Deck game = new(players, "test");
        game.DrawCard("Timmy");
        Card cardTimmy = game.DrawCard("Timmy");

        cardTimmy.ActiveColour = Colour.ALL;
        cardTimmy.ActiveValue = Value.DRAW_FOUR;

        game.Pile.ActiveColour = Colour.BLUE;

        game.UpdateGameState("Timmy", cardTimmy.ActiveValue, cardTimmy.ActiveColour, Colour.BLUE);

        Card[] handTimmy = game.Cards.Where(card =>
                                        card.Owner?.Name == "Timmy" &&
                                        !card.IsPlayed)
                                     .ToArray();

        Assert.Equal(5, handTimmy.Length);
    }
}
