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
            CardFactory cardFactory = new();

            //Act
            //cardFactory.InitialiseAllCards()

            //Assert
        }
    }
}
