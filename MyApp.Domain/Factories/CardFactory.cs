using MyApp.Domain.Enums;
using System.Runtime.CompilerServices;
using MyApp.Domain.EnumExtensionMethods;

[assembly: InternalsVisibleTo("MyApp.Domain.Tests")]
namespace MyApp.Domain.Factories
{
    internal class CardFactory : ICardFactory
    {
        private readonly Pile _pile;

        private readonly List<Card> _allCards = new();
        public Card[] GetAllCards()
        {
            if (_allCards.Count == 0)
            {
                InitialiseAllCards();
            }

            return _allCards.ToArray();
        }

        public CardFactory(Pile pile)
        {
            _pile = pile;
        }

        public Card[] InitialiseAllCards()
        {
            //every card is in the list twice, DRAW4 & CHANGE each have 4 copies, every colour has only ONE 0.
            CreateCardOfEachValueForColour(_pile, Colour.BLUE);
            CreateCardOfEachValueForColour(_pile, Colour.GREEN);
            CreateCardOfEachValueForColour(_pile, Colour.RED);
            CreateCardOfEachValueForColour(_pile, Colour.YELLOW);
            CreateColourChangeCards(_pile);

            return _allCards.ToArray();
        }

        private void CreateColourChangeCards(Pile pile)
        {
            CreateMultipleCards(4, pile, Colour.WILD, Value.CHANGE);
            CreateMultipleCards(4, pile, Colour.WILD, Value.DRAW4);
        }

        internal void CreateMultipleCards(int numberToCreate, Pile pile, Colour colour, Value value)
        {
            if (numberToCreate < 1)
            {
                return;
            }

            _allCards.Add(new Card(pile, colour, value));
            CreateMultipleCards(numberToCreate - 1, pile, colour, value);
        }

        private void CreateCardOfEachValueForColour(Pile pile, Colour colour)
        {
            if (colour == Colour.WILD)
            {
                throw new InvalidOperationException("Trying to create invalid cards");
            }

            var values = Enum.GetValues<Value>();

            foreach (var value in values)
            {
                if (value.IsInvalidForColours())
                {
                    continue;
                }

                var numberToCreate = value == Value.ZERO ? 1 : 2;
                CreateMultipleCards(numberToCreate, pile, colour, value);
            }
        }
    }
}
