using Moq;
using MyApp.Domain.Enums;
using MyApp.Domain.Factories;
using Xunit;
using static MyApp.Domain.Card;

namespace MyApp.Domain.Tests;

public class GameTest
{
    [Fact]
    public void TestGetPlayerNamesListReturnsListOfAllPlayerNames()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy", "Barney" };
        Game game = new(players);

        //Act
        var list = game.CreatePlayerList("Timmy");

        //Assert
        Assert.Equal(3, list.Length);
    }

    [Fact]
    public void TestGameEndsWhenPlayerHasNoCardsLeft()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy" };
        var pile = new Pile(players);

        var cardFactory = new Mock<ICardFactory>(MockBehavior.Strict);

        cardFactory
            .Setup(m => m.GetAllCards())
            .Returns(InitialiseTestCards(pile));

        cardFactory
            .Setup(m => m.GetPile())
            .Returns(pile);

        cardFactory
            .Setup(m => m.GetNumberOfCards())
            .Returns(0);

        Game game = new(players, cardFactory.Object);
        game.Deck.Pile.Owner!.Uno = true;
        Card card = game.Deck.DrawCard("Timmy");
        game.Deck.Pile.Owner.PreviousPlayer.Uno = true;

        //Act
        game.Deck.DrawCard("Jimmy");

        //Assert
        Assert.NotNull(game.UpdateGameState("Timmy", card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestGameStartsWithSevenCardsInEachHand()
    {
        //Arrange
        string[] players = { "Timmy" };
        Game game = new(players);

        //Act
        Card[] handTimmy = game.Deck.Cards.Where(unoCard =>
                unoCard.Owner?.Name == "Timmy" &&
                !unoCard.IsPlayed)
            .ToArray();

        //Assert
        Assert.Equal(7, handTimmy.Length);
    }

    [Fact]
    public void TestPlayingSkipAsSecondToLastCardResultsInValidUno()
    {
        //Arrange
        string[] players = { "Timmy", "Jimmy" };
        var pile = new Pile(players);

        var cardFactory = new Mock<ICardFactory>(MockBehavior.Strict);

        cardFactory
            .Setup(m => m.GetAllCards())
            .Returns(InitialiseTestCards(pile));

        cardFactory
            .Setup(m => m.GetPile())
            .Returns(pile);

        cardFactory
            .Setup(m => m.GetNumberOfCards())
            .Returns(0);

        Game game = new(players, cardFactory.Object);

        //Act
        Card cardTimmy = game.Deck.DrawCard("Timmy");
        cardTimmy.ActiveValue = Value.SKIP;
        game.Deck.DrawCard("Jimmy");

        Card otherCard = game.Deck.DrawCard("Timmy");
        game.Deck.DrawCard("Jimmy");

        Card[] playerHand = { cardTimmy, otherCard };
        PlayCard(game.Deck, playerHand, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.True(game.Deck.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestFalseUnoDrawsCards()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Game game = new(playerOne);

        //Act
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Card[] actual = game.Deck.Cards.Where(unoCard =>
                unoCard.Owner?.Name == "Timmy" &&
                !unoCard.IsPlayed)
            .ToArray();

        Assert.Equal(9, actual.Length);
    }

    [Fact]
    public void TestDrawCardUpdatesPlayerBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Game game = new(playerOne);

        //Act
        game.Deck.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");
        game.Deck.DrawCard("Timmy");

        //Assert
        Assert.False(game.Deck.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestInvalidUnoDoesNotUpdateBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Game game = new(playerOne);

        //Act
        game.Deck.DrawCard("Timmy");
        game.Deck.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.False(game.Deck.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    [Fact]
    public void TestValidUnoUpdatesPlayerBool()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        var pile = new Pile(playerOne);

        var cardFactory = new Mock<ICardFactory>(MockBehavior.Strict);

        cardFactory
            .Setup(m => m.GetAllCards())
            .Returns(InitialiseTestCards(pile));

        cardFactory
            .Setup(m => m.GetPile())
            .Returns(pile);

        cardFactory
            .Setup(m => m.GetNumberOfCards())
            .Returns(0);

        Game game = new(playerOne, cardFactory.Object);

        //Act
        game.Deck.DrawCard("Timmy");
        game.UnoButtonWasPressed("Timmy");

        //Assert
        Assert.True(game.Deck.Pile.Owner!.GetPlayerByName("Timmy").Uno);
    }

    private Card[] InitialiseTestCards(Pile pile)
    {
        Card[] testCards =
        {
            new (pile, Colour.RED, Value.ZERO),
            new (pile, Colour.RED, Value.ONE),
            new (pile, Colour.RED, Value.ONE),
            new (pile, Colour.RED, Value.TWO),
            new (pile, Colour.RED, Value.TWO),
            new (pile, Colour.RED, Value.THREE),
            new (pile, Colour.RED, Value.THREE),
            new (pile, Colour.RED, Value.FOUR),
            new (pile, Colour.RED, Value.FOUR),
            new (pile, Colour.RED, Value.FIVE),
            new (pile, Colour.RED, Value.FIVE),
            new (pile, Colour.RED, Value.SIX),
            new (pile, Colour.RED, Value.SIX),
            new (pile, Colour.RED, Value.SEVEN),
            new (pile, Colour.RED, Value.SEVEN),
            new (pile, Colour.RED, Value.EIGHT),
            new (pile, Colour.RED, Value.EIGHT),
            new (pile, Colour.RED, Value.NINE),
            new (pile, Colour.RED, Value.NINE),
        };
        return testCards;
    }
}

