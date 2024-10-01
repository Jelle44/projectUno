using MyApp.Domain.EnumExtensionMethods;
using MyApp.Domain.Enums;

namespace MyApp.Domain
{
    public class Card : CardSuperClass
    {
        public bool IsPlayed { get; internal set; }
        public Pile Pile { get; }

        internal Card(Pile pile, Colour colour, Value value)
        {
            Owner = null;
            Pile = pile;
            ActiveColour = colour;
            ActiveValue = value;
            IsPlayed = false;

            var colourString = ActiveColour.ToString().ToLower();

            if (CardHasNumberValue(value))
            {
                var valueInt = (int)value;
                Path = $"http://unocardinfo.victorhomedia.com/graphics/uno_card-{colourString}{valueInt}.png";
            }
            else
            {
                var valueString = ActiveValue.ToString().ToLower();
                Path = $"http://unocardinfo.victorhomedia.com/graphics/uno_card-{colourString}{valueString}.png";
            }
        }

        private static bool CardHasNumberValue(Value value)
        {
            return (int)value <= 9;
        }

        public static void PlayCard(Deck deck, Card[] playerHand, Value value, Colour colour, Colour newColour)
        {
            var selectedCard = GetSelectedCard(playerHand, value, colour);
            selectedCard.Pile.AddToPile(deck, selectedCard);
            selectedCard.Pile.ChangeColour(newColour);
        }

        internal void UpdateIsPlayed()
        {
            IsPlayed = true;
        }

        internal static Card GetSelectedCard(Card[] playerHand, Value value, Colour colour)
        {
            var playedCards = playerHand.Where(card =>
                                                    card.ActiveColour == colour &&
                                                    card.ActiveValue == value)
                                           .ToArray();

            return playedCards[0];
        }

        internal bool IsDrawCard()
        {
            return ActiveValue.IsDrawCard();
        }
    }
}
