using MyApp.Domain.Factories;
using Xunit;

namespace MyApp.Domain.Tests
{
    internal class CardFactoryTest
    {
        [Fact]
        public void InitialiseCardShouldReturnCard()
        {
            //Arrange
            CardFactory cardFactory = new();

            //Act
            //cardFactory.InitialiseAllCards()

            //Assert
        }
    }
}
