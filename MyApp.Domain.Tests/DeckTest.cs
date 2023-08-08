namespace MyApp.Domain.Tests;
using Xunit;
using MyApp.Domain;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static MyApp.Domain.CardSuperClass;
using static MyApp.Domain.Card;

public class DeckTest
{
    [Fact]
    public void TestUno()
    {
        string[] playerOne = { "Timmy" };
        Deck uno = new (playerOne);
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
        string[] playerOne = { "Timmy" };
        Deck game = new (playerOne);
        var hand = game.DrawCard("Timmy");
        Assert.True(hand.GetType() == typeof(Card));
    }

    [Fact]
    public void TestValidUnoUpdatesPlayerBool()
    {
        string[] playerOne = { "Timmy" };
        Deck game = new (playerOne, "testConstructor");
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        Assert.True(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestInvalidUnoDoesNotUpdateBool()
    {
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne, "Test constructor");
        game.DrawCard("Timmy");
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        Assert.False(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestDrawCardUpdatesPlayerBool()
    {
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne, "Test constructor");
        game.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");
        game.DrawCard("Timmy");

        Assert.False(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestFalseUnoDrawsCards()
    {
        string[] playerOne = { "Timmy" };
        Deck game = new(playerOne);
        game.UnoButtonWasPressed("Timmy");

        Card[] handTimmy = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(9, handTimmy.Length);
    }

    [Fact]
    public void TestPlayingSkipAsSecondToLastCardResultsInValidUno()
    {
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new(players, "testConstructor");

        Card cardTimmy = game.DrawCard("Timmy");
        cardTimmy.ActiveValue = Value.SKIPTURN;
        game.DrawCard("Jimmy");

        Card otherCard = game.DrawCard("Timmy");
        game.DrawCard("Jimmy");

        Card[] playerHand = { cardTimmy, otherCard };
        PlayCard(game, playerHand, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);
        game.UnoButtonWasPressed("Timmy");

        Assert.True(game.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestDeckReshufflesWhenEmpty()
    {
        string[] players = { "Timmy" };
        Deck game = new(players, "testConstructor");

        game.DrawMultipleCards("Timmy", 19);

        Card[] playerHand = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                      .ToArray();

        PlayCard(game, playerHand, Value.ZERO, Colour.RED, Colour.RED);

        game.DrawCard("Timmy");
        playerHand = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                               .ToArray();

        Assert.Equal(19, playerHand.Length);
    }

    [Fact]
    public void TestGameStartsWithSevenCardsInEachHand()
    {
        string[] players = { "Timmy" };
        Deck game = new(players);

        Card[] handTimmy = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(7, handTimmy.Length);
    }

    [Fact]
    public void TestGameEndsWhenPlayerHasNoCardsLeft()
    {
        string[] players = { "Timmy", "Jimmy" };
        Deck game = new(players, "Test Constructor");
        game.Pile.Owner!.Uno = true;
        Card card = game.DrawCard("Timmy");
        game.Pile.Owner.PreviousPlayer.Uno = true;
        game.DrawCard("Jimmy");

        Assert.NotNull(game.UpdateGameState("Timmy", card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestGEtPlayerNamesListReturnsListOfAllPlayerNames()
    {
        string[] players = { "Timmy", "Jimmy", "Barney" };
        Deck game = new(players);
        string[] list = game.CreatePlayerList("Timmy");

        Assert.Equal(3, list.Length);
    }
}
