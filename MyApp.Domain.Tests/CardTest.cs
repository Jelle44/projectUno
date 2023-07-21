using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static MyApp.Domain.CardSuperClass;
using static MyApp.Domain.Card;
using System.Xml.Linq;

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
        Deck game = new (players, "Test constructor");
        Card card = game.DrawCard("Timmy");
        game.DrawCard("Jimmy");

        Card[] playerHand = { card };
        PlayCard(game, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour);

        Assert.Equal(card.Owner, game.Pile.Owner);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPlayCardChecksForUno()
    {
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new (players, "testConstructor");

        Card cardTimmy = game.DrawCard("Timmy");
        game.DrawCard("Jimmy");
        game.DrawCard("Timmy");

        game.Pile.Owner = game.Pile.Owner!.GetPlayerByName("Jimmy");
        game.UpdateGameState("Timmy", cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);

        Card[] handJimmy = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Jimmy" &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(3, handJimmy.Length);
    }
}
