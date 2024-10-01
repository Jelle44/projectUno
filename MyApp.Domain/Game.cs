using MyApp.Domain.Enums;
using MyApp.Domain.Factories;

namespace MyApp.Domain;

public class Game
{
    public Deck Deck { get; }


    public Game(string[] players)
        : this (players, new CardFactory(new Pile(players)))
    {
    }

    internal Game(string[] players, ICardFactory cardFactory)
    {
        Deck = new Deck(cardFactory);

        if (Deck.Counter > 0)
        {
            StartGame(players);
        }
    }

    private void StartGame(string[] players)
    {
        foreach (var player in players)
        {
            Deck.DrawMultipleCards(player, 7);
        }

        Deck.Pile.SetPileTopCardForStartGame(Deck, players);
    }

    internal void CheckForgottenUno(string playerName)
    {
        if (CheckUno(playerName) && !PlayerHasUno(playerName))
        {
            Deck.DrawTwoCards(playerName);
        };
    }

    public void UnoButtonWasPressed(string playerName)
    {
        if (UnoIsValid(playerName))
        {
            Deck.Pile.Owner!.GetPlayerByName(playerName).UpdateUno();
            return;
        }

        Deck.DrawTwoCards(playerName);
    }

    private bool PlayerHasUno(string playerName)
    {
        return Deck.Pile.Owner!.GetPlayerByName(playerName).Uno;
    }


    private bool UnoIsValid(string playerName)
    {
        return CheckUno(playerName);
    }

    private bool CheckUno(string playerName)
    {
        var playerHand = GetPlayerHand(playerName);
        var playerWithPossibleUno = Deck.Pile.Owner!.GetPlayerByName(playerName);

        var playerOfLastCard = Deck.Pile.Owner;

        if (Deck.Pile.ActiveValue == Value.SKIP)
        {
            playerOfLastCard = Deck.Pile.GetOwnerOfSkipTurn();
        }

        return (playerHand.Length <= 1 &&
                playerWithPossibleUno == playerOfLastCard);
    }

    public string? UpdateGameState(string name, Value value, Colour colour, Colour newColour)
    {
        CheckForgottenUno(Deck.Pile.Owner!.Name);
        var playerHand = GetPlayerHand(name);
        Card.PlayCard(Deck, playerHand, value, colour, newColour);

        if (PlayerHasNoCardsInHand())
        {
            return EndGame();
        }

        return null;
    }

    private string EndGame()
    {
        foreach (var card in Deck.Cards)
        {
            card.IsPlayed = true;
        }

        return Winner();
    }

    private string Winner()
    {
        return Deck.Pile.Owner!.Name;
    }

    public bool PlayerHasNoCardsInHand()
    {
        var owner = Deck.Pile.Owner;

        if (Deck.Pile.ActiveValue == Value.SKIP)
        {
            owner = Deck.Pile.GetOwnerOfSkipTurn();
        }

        return (Deck.Cards
            .Where(card =>
                card.Owner == owner &&
                !card.IsPlayed)
            .ToArray()
            .Length == 0);
    }

    public Card[] GetPlayerHand(string name)
    {
        return Deck.Cards
            .Where(unoCard =>
                unoCard.Owner?.Name == name &&
                !unoCard.IsPlayed)
            .ToArray();
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
        name = Deck.Pile.Owner!.GetPlayerByName(name).NextPlayer.Name;

        if (name != playerNames[0])
        {
            playerNames.Add(name);
            GetAllPlayers(playerNames, name);
        }

        return playerNames;
    }
}
