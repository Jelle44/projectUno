using FluentAssertions;
using Moq;
using MyApp.Domain.Enums;
using MyApp.Domain.Factories;
using Xunit;
using static MyApp.Domain.Card;

namespace MyApp.Domain.Tests;
public class CardTest
{
    [Fact]
    public void TestCardExists()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] players = { playerName };
        Pile pile = new(players);

        //Act
        Card actual = new(pile, Colour.RED, Value.EIGHT);

        //Assert
        Assert.NotNull(actual);
    }

    [Fact]
    public void TestCardHasOwner()
    {
        //Arrange
        const string expectedName = "Timmy";
        string[] players = { expectedName };
        Pile pile = new(players);

        //Act
        Card actual = new(pile, Colour.RED, Value.EIGHT)
        {
            Owner = pile.Owner
        };

        //Assert
        Assert.Equal(expectedName, actual.Owner!.Name);
    }

    [Fact]
    public void TestCardHasActiveColour()
    {
        //Arrange
        const Colour expectedColour = Colour.BLUE;
        const string playerName = "Timmy";
        string[] players = { playerName };
        Pile pile = new(players);

        //Act
        Card card = new(pile, Colour.BLUE, Value.EIGHT);

        //Assert
        Assert.Equal(expectedColour, card.ActiveColour);
    }

    [Fact]
    public void TestCardHasActiveValue()
    {
        //Arrange
        const Value expectedValue = Value.EIGHT;
        const string playerName = "Timmy";
        string[] players = { playerName };
        Pile pile = new(players);

        //Act
        Card card = new(pile, Colour.RED, Value.EIGHT);

        //Assert
        Assert.Equal(expectedValue, card.ActiveValue);
    }

    [Fact]
    public void TestCardHasBoolIsPlayed()
    {
        //Arrange
        const string playerName = "Timmy";
        string[] players = { playerName };
        Pile pile = new(players);

        //Act
        Card card = new(pile, Colour.RED, Value.EIGHT);

        //Assert
        Assert.False(card.IsPlayed);
    }

    [Fact]
    public void TestPlayCardUpdatesPile()
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

        Game game = new (players, cardFactory.Object);
        var card = game.Deck.DrawCard(playerOneName);
        game.Deck.DrawCard(playerTwoName);

        Card[] playerHand = { card };

        //Act
        PlayCard(game.Deck, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour);

        //Assert
        Assert.Equal(card.Owner, game.Deck.Pile.Owner);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPlayCardChecksForUno()
    {
        //Arrange
        const int expectedHandSize = 3;
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

        Game game = new (players, cardFactory.Object);

        var cardTimmy = game.Deck.DrawCard(playerOneName);
        game.Deck.DrawCard(playerTwoName);
        game.Deck.DrawCard(playerOneName);

        game.Deck.Pile.Owner = game.Deck.Pile.Owner!.GetPlayerByName(playerTwoName);

        //Act
        game.UpdateGameState(playerOneName, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);

        //Assert
        var actual = game.Deck.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == playerTwoName &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(expectedHandSize, actual.Length);
    }

    [Fact]
    public void WhenColourAndNormalValueArePassedCardConstructorShouldSetPathWithCorrectColourAndValue()
    {
        //Arrange
        const string expected = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red0.png";

        //Act
        Card card = new(null, Colour.RED, Value.ZERO);

        //Assert
        var actual = card.Path;
        actual.Should().NotBeNull();
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png", Colour.WILD, Value.CHANGE)]
    [InlineData("http://unocardinfo.victorhomedia.com/graphics/uno_card-redskip.png", Colour.RED, Value.SKIP)]
    [InlineData("http://unocardinfo.victorhomedia.com/graphics/uno_card-redreverse.png", Colour.RED, Value.REVERSE)]
    [InlineData("http://unocardinfo.victorhomedia.com/graphics/uno_card-reddraw2.png", Colour.RED, Value.DRAW2)]
    [InlineData("http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png", Colour.WILD, Value.DRAW4)]
    public void WhenChangeValueCardIsPassedCardConstructorShouldSetPathWithCorrectValue(string expected, Colour colour, Value value)
    {
        //Act
        Card card = new(null, colour, value);

        //Assert
        var actual = card.Path;
        actual.Should().NotBeNull();
        actual.Should().Be(expected);
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
