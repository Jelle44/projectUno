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
        string[] playerOne = { "Timmy" };

        //Act
        Game uno = new(playerOne);

        //Assert
        Assert.Equal(100, uno.Deck.Counter);
    }

    [Fact]
    public void TestUnoCounter()
    {
        Assert.Equal(107, Deck.DecreaseCounter(108));
    }

    [Fact]
    public void TestDrawCardCreatesCard()
    {
        //Arrange
        string[] playerOne = { "Timmy" };
        Game game = new(playerOne);

        //Act
        var hand = game.Deck.DrawCard("Timmy");

        //Assert
        Assert.True(hand.GetType() == typeof(Card));
    }

    [Fact]
    public void TestDeckReshufflesWhenEmpty()
    {
        //Arrange
        string[] players = { "Timmy" };
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

        game.Deck.DrawMultipleCards("Timmy", 19);

        Card[] playerHand = game.Deck.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                                      .ToArray();

        //Act
        PlayCard(game.Deck, playerHand, Value.ZERO, Colour.RED, Colour.RED);
        game.Deck.DrawCard("Timmy");

        //Assert
        playerHand = game.Deck.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == "Timmy" &&
                                            !unoCard.IsPlayed)
                               .ToArray();

        Assert.Equal(19, playerHand.Length);
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
