using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyApp.Domain.Deck;
using static MyApp.Domain.Pile;
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

        card.PlayCard();

        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
    }

    [Fact]
    public void TestPileAllowsSameValueDifferentColour()
    {
        string[] players = { "Timmy" };
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

        card.PlayCard();

        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPileDoesNotAllowDifferentColour()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile)
        {
            Owner = new Player(players),
            ActiveValue = Value.ZERO,
            ActiveColour = Colour.BLUE
        };

        pile.ActiveValue = Value.ONE;
        pile.ActiveColour = Colour.RED;

        Assert.Throws<InvalidCardException>(() => card.PlayCard());
    }
}
