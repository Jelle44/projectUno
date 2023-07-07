using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static MyApp.Domain.Deck;

namespace MyApp.Domain.Tests;
public class CardTest
{
    [Fact]
    public void TestCardExists()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile);
        Assert.NotNull(card);
    }

    [Fact]
    public void TestCardHasOwner()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile);
        card.Owner = pile.Owner;
        Assert.Equal("Timmy", card.Owner!.Name);
    }

    [Fact]
    public void TestCardHasActiveColour()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile);
        Assert.Equal(Colour.BLUE, card.ActiveColour);
    }

    [Fact]
    public void TestCardHasActiveValue()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile);
        Assert.Equal(Value.ZERO, card.ActiveValue);
    }

    [Fact]
    public void TestCardHasBoolIsPlayed()
    {
        string[] players = { "Timmy" };
        Pile pile = new(players);
        Card card = new(pile);
        Assert.False(card.IsPlayed);
    }

    [Fact]
    public void TestPlayCardUpdatesPile()
    {
        string[] players = { "Timmy", "Jimmy" };
        Pile pile = new(players)
        {
            //owner is last name in players-array
            ActiveColour = Colour.BLUE,
            ActiveValue = Value.FOUR
        };
        Card card = new(pile)
        {
            Owner = pile.Owner!.NextPlayer, //should be the first player in players-array
            ActiveColour = Colour.BLUE,
            ActiveValue = Value.FIVE
        };
        card.PlayCard();
        pile = card.Pile;
        Assert.Equal(card.Owner, pile.Owner);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }
}
