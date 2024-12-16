using FluentAssertions;
using MyApp.Domain.Enums;
using MyApp.Domain.Factories;
using Xunit;

namespace MyApp.Domain.Tests
{
    public class CardFactoryTest
    {
        [Fact]
        public void InitialiseCardShouldReturnDeck()
        {
            //Arrange
            const int expectedNumberOfCards = 108;
            CardFactory cardFactory = new(null);

            //Act
            var actual = cardFactory.GetAllCards();

            //Assert
            actual.Length.Should().Be(expectedNumberOfCards);
        }

        [Fact]
        public void CreateMultipleCardsShouldCreateCardsEqualToGivenAmount()
        {
            //Arrange
            const int expectedCount = 2;
            const string playerName = "Jimmy";
            string[] playerNames = { playerName };
            Pile pile = new(playerNames);
            CardFactory cardFactory = new(null);

            //Act
            cardFactory.CreateMultipleCards(expectedCount, pile, Colour.RED, Value.DRAW2);

            //Assert
            cardFactory.GetAllCards().Length.Should().Be(expectedCount);
        }
    }
}
