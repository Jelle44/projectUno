using Moq;
using MyApp.Domain.Enums;
using MyApp.Domain.Factories;
using Xunit;
using static MyApp.Domain.Card;

namespace MyApp.Domain.Tests;

public class DeckTest
{
    [Fact]
    public void TestUno()
    {
        //Arrange
        const int expectedDeckSizeAfterStartingHandsAndPileAreRemoved = 100;
        const string playerName = "Timmy";
        string[] playerOne = { playerName };

        //Act
        Game game = new(playerOne);

        //Assert
        Assert.Equal(expectedDeckSizeAfterStartingHandsAndPileAreRemoved, game.Deck.Counter);
    }

    [Fact]
    public void TestUnoCounter()
    {
        //Arrange
        const int expectedCounter = 107;
        const int startingNumber = 108;

        //Act
        var actual = Deck.DecreaseCounter(startingNumber);

        Assert.Equal(expectedCounter, actual);
    }

    [Fact]
    public void TestDrawCardCreatesCard()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] playerOne = { playerName };
        Game game = new(playerOne);

        //Act
        var hand = game.Deck.DrawCard(playerName);

        //Assert
        Assert.True(hand.GetType() == typeof(Card));
    }

    [Fact]
    public void TestDeckReshufflesWhenEmpty()
    {
        //Arrange
        const int expectedAmountOfCardsDrawn = 19;
        const int numberOfCardsToDraw = 19;
        const string playerName = "Timmy";
        string[] players = { playerName };
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

        game.Deck.DrawMultipleCards(playerName, numberOfCardsToDraw);

        var playerHand = game.Deck.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == playerName &&
                                            !unoCard.IsPlayed)
                                      .ToArray();

        //Act
        PlayCard(game.Deck, playerHand, Value.ZERO, Colour.RED, Colour.RED);
        game.Deck.DrawCard(playerName);

        //Assert
        playerHand = game.Deck.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == playerName &&
                                            !unoCard.IsPlayed)
                               .ToArray();

        Assert.Equal(expectedAmountOfCardsDrawn, playerHand.Length);
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
