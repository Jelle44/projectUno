using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static MyApp.Domain.Pile;
using static MyApp.Domain.Deck;

namespace MyApp.Domain
{
    public class Card : CardSuperClass
    {
        public bool IsPlayed { get; set; }
        public Pile Pile { get; private set; }
        public Card(Pile pile)
        {
            this.Owner = null;
            this.Pile = pile;
            this.ActiveColour = Colour.BLUE;
            this.ActiveValue = Value.ZERO;
            this.IsPlayed = false;
            this.Path = "";
        }

        public static void PlayCard(Deck deck, Card[] playerHand, CardSuperClass.Value value, CardSuperClass.Colour colour, CardSuperClass.Colour newColour)
        {
            Card selectedCard = GetSelectedCard(playerHand, value, colour);
            selectedCard.Pile.AddToPile(deck, selectedCard);
            selectedCard.Pile.ChangeColour(newColour);
        }

        internal void UpdateIsPlayed()
        {
            this.IsPlayed = true;
        }

        internal static Card GetSelectedCard(Card[] playerHand, CardSuperClass.Value value, CardSuperClass.Colour colour)
        {
            Card[] playedCards = playerHand.Where(card =>
                                                    card.ActiveColour == colour &&
                                                    card.ActiveValue == value)
                                           .ToArray();

            return playedCards[0];
        }
    }
}
