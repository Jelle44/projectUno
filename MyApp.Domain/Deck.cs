using MyApp.Domain.Exceptions;
using System.Diagnostics;
using static MyApp.Domain.CardSuperClass;

namespace MyApp.Domain;

public class Deck
{
    public int Counter { get ; set; }

    public Pile Pile { get; }
    public Card[] Cards { get; }
    public Deck(string[] players)
    {
        this.Counter = 108;
        this.Pile = new Pile(players);
        this.Cards = InitialiseAllCards(this.Pile);

        foreach (string player in players)
        {
            DrawMultipleCards(player, 7);
        }

        this.Pile.SetPileTopCardForStartGame(this, players);
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
        if (this.Pile.DrawIsValid(playerName))
        {
            CheckNumCardsInHand(playerName);
            this.Pile.Owner = this.Pile.ChangeTurn();

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
        if(PlayerHasUno(playerName))
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

        if(Pile.ActiveValue == Value.SKIPTURN)
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
        return this.Cards.Where(card =>
                                card.Owner == null &&
                                !card.IsPlayed)
                         .ToArray();
    }

    private static int GetRandomNumber(Card[] cards)
    {
        Random rnd = new();
        return rnd.Next(0, (cards.Length));
    }

    public string? UpdateGameState(string name, CardSuperClass.Value value, CardSuperClass.Colour colour, CardSuperClass.Colour newColour)
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
        foreach (Card card in this.Cards)
        {
            card.IsPlayed = true;
        }

        return Winner();
    }

    private string Winner()
    {
        return this.Pile.Owner!.Name;
    }

    public bool PlayerHasNoCardsInHand()
    {
        var owner = this.Pile.Owner;

        if(Pile.ActiveValue == Value.SKIPTURN)
        {
            owner = Pile.GetOwnerOfSkipTurn();
        }

        return (this.Cards.Where(card =>
                                    card.Owner == owner &&
                                    !card.IsPlayed)
                          .ToArray()
                          .Length == 0);
    }

    public Card[] GetPlayerHand(string name)
    {
        return this.Cards.Where(unoCard =>
                                 unoCard.Owner?.Name == name &&
                                 !unoCard.IsPlayed)
                         .ToArray();
    }

    private static Card[] InitialiseAllCards(Pile pile)
    { //every card is in the list twice, DRAW_FOUR & RECOLOUR each have 4 copies, every colour has only ONE 0.
        Card[] allCards = new Card[]
                {
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red0.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red1.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red1.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red3.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red3.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red4.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red4.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red5.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red5.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red6.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red6.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red7.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red7.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red8.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red8.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-reddraw2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-reddraw2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redreverse.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redreverse.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redskip.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redskip.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow0.png"},
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow1.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow1.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow3.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow3.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow4.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow4.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow5.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow5.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow6.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow6.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow7.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow7.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow8.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow8.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow9.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow9.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowdraw2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowdraw2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowreverse.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowreverse.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowskip.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowskip.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green0.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green1.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green1.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green3.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green3.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green4.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green4.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green5.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green5.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green6.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green6.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green7.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green7.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green8.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green8.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green9.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green9.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greendraw2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greendraw2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenreverse.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenreverse.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenskip.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenskip.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue0.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue1.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue1.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue3.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue3.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue4.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue4.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue5.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue5.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue6.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue6.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue7.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue7.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue8.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue8.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue9.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue9.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluedraw2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluedraw2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluereverse.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluereverse.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blueskip.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blueskip.png" },
                new Card(pile) {ActiveColour = Colour.ALL, ActiveValue = Value.RECOLOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png"},
                new Card(pile) {ActiveColour = Colour.ALL, ActiveValue = Value.RECOLOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png"},
                new Card(pile) {ActiveColour = Colour.ALL, ActiveValue = Value.RECOLOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png"},
                new Card(pile) {ActiveColour = Colour.ALL, ActiveValue = Value.RECOLOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png"},
                new Card(pile) {ActiveColour = Colour.ALL,ActiveValue = Value.DRAW_FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png"},
                new Card(pile) {ActiveColour = Colour.ALL,ActiveValue = Value.DRAW_FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png"},
                new Card(pile) {ActiveColour = Colour.ALL,ActiveValue = Value.DRAW_FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png"},
                new Card(pile) {ActiveColour = Colour.ALL,ActiveValue = Value.DRAW_FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png"}
                };
        return allCards;
    }

    private static Card[] InitialiseTestCards(Pile pile)
    {
        Card[] testCards = new Card[]
                {
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red0.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red1.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red1.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red3.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red3.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red4.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red4.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red5.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red5.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red6.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red6.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red7.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red7.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red8.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red8.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
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
