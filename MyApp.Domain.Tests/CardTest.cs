using FluentAssertions;
using MyApp.Domain.Enums;
using Xunit;
using static MyApp.Domain.Card;

namespace MyApp.Domain.Tests;
public class CardTest
{
    [Fact]
    public void TestCardExists()
    {
        //Arrange
        string[] players = { "Timmy" };
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
        string[] players = { "Timmy" };
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
        string[] players = { "Timmy" };
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
        string[] players = { "Timmy" };
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
        Deck game = new (players, "Test constructor");
        Card card = game.DrawCard(playerOneName);
        game.DrawCard(playerTwoName);

        Card[] playerHand = { card };

        //Act
        PlayCard(game, playerHand, card.ActiveValue, card.ActiveColour, card.ActiveColour);

        //Assert
        Assert.Equal(card.Owner, game.Pile.Owner);
        Assert.Equal(card.ActiveValue, card.Pile.ActiveValue);
        Assert.Equal(card.ActiveColour, card.Pile.ActiveColour);
    }

    [Fact]
    public void TestPlayCardChecksForUno()
    {
        //Arrange
        const string playerOneName = "Timmy";
        const string playerTwoName = "Jimmy";
        string[] players = { playerOneName, playerTwoName };
        Deck game = new (players, "testConstructor");

        Card cardTimmy = game.DrawCard(playerOneName);
        game.DrawCard(playerTwoName);
        game.DrawCard(playerOneName);

        game.Pile.Owner = game.Pile.Owner!.GetPlayerByName(playerTwoName);

        //Act
        game.UpdateGameState(playerOneName, cardTimmy.ActiveValue, cardTimmy.ActiveColour, cardTimmy.ActiveColour);

        //Assert
        Card[] actual = game.Cards.Where(unoCard =>
                                            unoCard.Owner?.Name == playerTwoName &&
                                            !unoCard.IsPlayed)
                                     .ToArray();

        Assert.Equal(3, actual.Length);
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
}
