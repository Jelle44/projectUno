using MyApp.Domain.Enums;
using MyApp.Domain.Exceptions;
using MyApp.Domain.Factories;

namespace MyApp.Domain;

public class Deck
{
    public int Counter { get; set; }

    public Pile Pile { get; }
    public Card[] Cards { get; }

    public Deck(string[] players)
        : this(players, new CardFactory(new Pile(players)))
    {
    }

    private Deck(string[] players, ICardFactory cardFactory)
    {
        Pile = cardFactory.GetPile();
        Cards = cardFactory.GetAllCards();
        Counter = Cards.Length;

        foreach (var player in players)
        {
            DrawMultipleCards(player, 7);
        }

        Pile.SetPileTopCardForStartGame(this, players);
    }

    public Deck(string[] players, string testConstructor)
    {
        this.Counter = 108;
        this.Pile = new Pile(players);
        this.Cards = InitialiseTestCards(this.Pile);
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

    internal void CheckForgottenUno(string playerName)
    {
        if (CheckUno(playerName) && !PlayerHasUno(playerName))
        {
            DrawTwoCards(playerName);
        };
    }

    private void CheckNumCardsInHand(string playerName)
    {
        if (PlayerHasUno(playerName))
        {
            Pile.Owner!.GetPlayerByName(playerName).UpdateUno();
        }
    }

    public void UnoButtonWasPressed(string playerName)
    {
        if (UnoIsValid(playerName))
        {
            Pile.Owner!.GetPlayerByName(playerName).UpdateUno();
            return;
        }

        DrawTwoCards(playerName);
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

    private bool UnoIsValid(string playerName)
    {
        return CheckUno(playerName);
    }

    private bool CheckUno(string playerName)
    {
        Card[] playerHand = GetPlayerHand(playerName);
        Player playerWithPossibleUno = Pile.Owner!.GetPlayerByName(playerName);

        Player playerOfLastCard = Pile.Owner;

        if (Pile.ActiveValue == Value.SKIP)
        {
            playerOfLastCard = Pile.GetOwnerOfSkipTurn();
        }

        return (playerHand.Length <= 1 &&
                playerWithPossibleUno == playerOfLastCard);
    }

    internal Card GetCompleteCard(string playerName)
    {
        Card[] activeCards = GetActiveCards();

        if (activeCards.Length == 0)
        {
            activeCards = ReshuffleDeck();
            this.Counter = activeCards.Length;
        }

        Card drawnCard = activeCards[GetRandomNumber(activeCards)];
        drawnCard.Owner = Pile.Owner!.GetPlayerByName(playerName);

        this.Counter = DecreaseCounter(this.Counter);

        return drawnCard;
    }

    private Card[] ReshuffleDeck()
    {
        Card[] shuffledDeck = this.Cards.Where(card => card.IsPlayed).ToArray();

        foreach (Card card in shuffledDeck)
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

    private static int GetRandomNumber(Card[] cards)
    {
        Random rnd = new();
        return rnd.Next(0, (cards.Length));
    }

    public string? UpdateGameState(string name, Value value, Colour colour, Colour newColour)
    {
        CheckForgottenUno(Pile.Owner!.Name);
        Card[] playerHand = GetPlayerHand(name);
        Card.PlayCard(this, playerHand, value, colour, newColour);

        if (PlayerHasNoCardsInHand())
        {
            return EndGame();
        }

        return null;
    }

    private string EndGame()
    {
        foreach (Card card in Cards)
        {
            card.IsPlayed = true;
        }

        return Winner();
    }

    private string Winner()
    {
        return Pile.Owner!.Name;
    }

    public bool PlayerHasNoCardsInHand()
    {
        var owner = Pile.Owner;

        if (Pile.ActiveValue == Value.SKIP)
        {
            owner = Pile.GetOwnerOfSkipTurn();
        }

        return (Cards
            .Where(card =>
                card.Owner == owner &&
                !card.IsPlayed)
            .ToArray()
            .Length == 0);
    }

    public Card[] GetPlayerHand(string name)
    {
        return Cards
            .Where(unoCard => 
                unoCard.Owner?.Name == name &&
                !unoCard.IsPlayed)
            .ToArray();
    }

    internal static Card[] InitialiseTestCards(Pile pile)
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

    public string[] CreatePlayerList(string playerId)
    {
        List<string> playerNames = new()
        {
            playerId
        };

        playerNames = GetAllPlayers(playerNames, playerId);

        return playerNames.ToArray();
    }

    private List<string> GetAllPlayers(List<string> playerNames, string name)
    {
        name = Pile.Owner!.GetPlayerByName(name).NextPlayer.Name;

        if (name != playerNames[0])
        {
            playerNames.Add(name);
            GetAllPlayers(playerNames, name);
        }

        return playerNames;
    }
}
