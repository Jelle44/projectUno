using FluentAssertions;
using MyApp.Domain.Enums;
using MyApp.Domain.Factories;
using Xunit;

namespace MyApp.Domain.Tests
{
    public class CardFactoryTest
    {
        [Fact]
        public void InitialiseCardShouldReturnCard()
        {
            //Arrange
            CardFactory cardFactory = new(null);

            //Act
            //cardFactory.InitialiseAllCards()

            //Assert
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
