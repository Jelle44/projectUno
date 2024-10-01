using MyApp.Domain.Exceptions;
using MyApp.Domain.Factories;

namespace MyApp.Domain;

public class Deck
{
    public int Counter { get; private set; }

    public Pile Pile { get; }
    public Card[] Cards { get; }

    internal Deck(ICardFactory cardFactory)
    {
        Pile = cardFactory.GetPile();
        Cards = cardFactory.GetAllCards();
        Counter = cardFactory.GetNumberOfCards();
    }

    public static int DecreaseCounter(int number)
    {
        number--;

        return number;
    }

    public Card DrawCard(string playerName)
    {
        if (Pile.DrawIsValid(playerName))
        {
            CheckNumCardsInHand(playerName);
            Pile.Owner = Pile.ChangeTurn();

            return GetCompleteCard(playerName);
        }

        throw new NotYourTurnException();
    }

    private void CheckNumCardsInHand(string playerName)
    {
        if (PlayerHasUno(playerName))
        {
            Pile.Owner!.GetPlayerByName(playerName).UpdateUno();
        }
    }

    private bool PlayerHasUno(string playerName)
    {
        return Pile.Owner!.GetPlayerByName(playerName).Uno;
    }

    internal void DrawTwoCards(string playerName)
    {
        DrawMultipleCards(playerName, 2);
    }

    public void DrawMultipleCards(string playerName, int numberOfCardsToDraw)
    {
        if (numberOfCardsToDraw > 0)
        {
            GetCompleteCard(playerName);
            DrawMultipleCards(playerName, (numberOfCardsToDraw - 1));
        }
    }

    internal Card GetCompleteCard(string playerName)
    {
        var activeCards = GetActiveCards();

        if (activeCards.Length == 0)
        {
            activeCards = ReshuffleDeck();
            Counter = activeCards.Length;
        }

        var drawnCard = activeCards[GetRandomNumber(activeCards)];
        drawnCard.Owner = Pile.Owner!.GetPlayerByName(playerName);

        Counter = DecreaseCounter(Counter);

        return drawnCard;
    }

    private Card[] ReshuffleDeck()
    {
        var shuffledDeck = Cards.Where(card => card.IsPlayed).ToArray();

        foreach (var card in shuffledDeck)
        {
            card.IsPlayed = false;
            card.Owner = null;
        }

        return shuffledDeck;
        //TODO: this will likely result in a bug, where if you draw the card that was on top of the pile, before the shuffle,
        //you will no longer have it when you play a card. The card itself might even be unable to be played.

        //TODO: another bug: if a player has the entire deck as hand, drawing a card is impossible.
    }

    private Card[] GetActiveCards()
    {
        return Cards
            .Where(card =>
                card.Owner == null &&
                !card.IsPlayed)
            .ToArray();
    }

    private static int GetRandomNumber(IReadOnlyCollection<Card> cards)
    {
        Random rnd = new();
        return rnd.Next(0, (cards.Count));
    }
}
