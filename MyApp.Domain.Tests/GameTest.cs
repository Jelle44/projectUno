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
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        const string playerThreeName = "Barney";
        string[] players = { playerOneName, playerTwoName, playerThreeName };
        Game game = new(players);

        //Act
        var list = game.CreatePlayerList(playerOneName);

        //Assert
        Assert.Equal(3, list.Length);
    }

    [Fact]
    public void TestGameEndsWhenPlayerHasNoCardsLeft()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] players = { playerOneName, playerTwoName };
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
        var card = game.Deck.DrawCard(playerOneName);
        game.Deck.Pile.Owner.PreviousPlayer.Uno = true;

        //Act
        game.Deck.DrawCard(playerTwoName);

        //Assert
        Assert.NotNull(game.UpdateGameState(playerOneName, card.ActiveValue, card.ActiveColour, card.ActiveColour));
    }

    [Fact]
    public void TestGameStartsWithSevenCardsInEachHand()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] players = { playerName };
        Game game = new(players);

        //Act
        var handTimmy = game.Deck.Cards.Where(unoCard =>
                unoCard.Owner?.Name == playerName &&
                !unoCard.IsPlayed)
            .ToArray();

        //Assert
        Assert.Equal(7, handTimmy.Length);
    }

    [Fact]
    public void TestPlayingSkipAsSecondToLastCardResultsInValidUno()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] players = { playerOneName, playerTwoName };
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
        var cardTimmy = game.Deck.DrawCard(playerOneName);
        cardTimmy.ActiveValue = Value.SKIP;
        game.Deck.DrawCard(playerTwoName);

        var otherCard = game.Deck.DrawCard(playerOneName);
        game.Deck.DrawCard(playerTwoName);

        Card[] playerHand = { cardTimmy, otherCard };
        PlayCard(game.Deck, playerHand, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);
        game.UnoButtonWasPressed(playerOneName);

        //Assert
        Assert.True(game.Deck.Pile.Owner!.GetPlayerByName(playerOneName).Uno);
    }

    [Fact]
    public void TestFalseUnoDrawsCards()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] playerOne = { playerName };
        Game game = new(playerOne);

        //Act
        game.UnoButtonWasPressed(playerName);

        //Assert
        var actual = game.Deck.Cards.Where(unoCard =>
                unoCard.Owner?.Name == playerName &&
                !unoCard.IsPlayed)
            .ToArray();

        Assert.Equal(9, actual.Length);
    }

    [Fact]
    public void TestDrawCardUpdatesPlayerBool()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] playerOne = { playerName };
        Game game = new(playerOne);

        //Act
        game.Deck.DrawCard(playerName);
        game.UnoButtonWasPressed(playerName);
        game.Deck.DrawCard(playerName);

        //Assert
        Assert.False(game.Deck.Pile.Owner!.GetPlayerByName(playerName).Uno);
    }

    [Fact]
    public void TestInvalidUnoDoesNotUpdateBool()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] playerOne = { playerName };
        Game game = new(playerOne);

        //Act
        game.Deck.DrawCard(playerName);
        game.Deck.DrawCard(playerName);
        game.UnoButtonWasPressed(playerName);

        //Assert
        Assert.False(game.Deck.Pile.Owner!.GetPlayerByName(playerName).Uno);
    }

    [Fact]
    public void TestValidUnoUpdatesPlayerBool()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] playerOne = { playerName };
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
        game.Deck.DrawCard(playerName);
        game.UnoButtonWasPressed(playerName);

        //Assert
        Assert.True(game.Deck.Pile.Owner!.GetPlayerByName(playerName).Uno);
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

