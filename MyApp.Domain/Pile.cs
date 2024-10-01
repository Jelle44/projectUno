using MyApp.Domain.Enums;
using MyApp.Domain.Exceptions;

namespace MyApp.Domain;
public class Pile : CardSuperClass
{
    public bool TurnOrderIsReversed { get; set; }

    public Pile(IReadOnlyList<string> names)
    {
        Owner = new Player(names);
        ActiveColour = Colour.WILD;
        ActiveValue = Value.FOUR;
        TurnOrderIsReversed = false;
        Path = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/fed3bb24-454f-4bdf-a721-6aa8f23e7cef/d9gnihf-ec16caeb-ec9c-4870-9480-57c7711d844f.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2ZlZDNiYjI0LTQ1NGYtNGJkZi1hNzIxLTZhYThmMjNlN2NlZlwvZDlnbmloZi1lYzE2Y2FlYi1lYzljLTQ4NzAtOTQ4MC01N2M3NzExZDg0NGYucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.kp5EommPFQl1sDPPtC-p8JloXDTm3CyNUgoievwh8Kw";
    }

    internal void AddToPile(Deck deck, Card card)
    {
        if (IsCardAllowed(card))
        {
            ChangePileTopCard(deck, card);
            card.UpdateIsPlayed();
        }
        else if (CheckNotYourTurnException(card))
        {
            throw new NotYourTurnException();
        }
        else
        {
            throw new InvalidCardException();
        }
    }

    private bool CheckNotYourTurnException(CardSuperClass card)
    {
        return (GetCurrentTurnHolder() != card.Owner);
    }

    private void ChangePileTopCard(Deck deck, Card card)
    {
        ActiveColour = card.ActiveColour;
        ActiveValue = card.ActiveValue;

        if (card.IsDrawCard())
        {
            DrawCorrectAmountOfCards(deck, card);
        }

        TurnOrderIsReversed = CheckChangeTurnOrder(card.ActiveValue);
        Path = card.Path;
        Owner = GetNewOwner(card);
    }

    private void DrawCorrectAmountOfCards(Deck deck, CardSuperClass card)
    {
        if(card.ActiveValue == Value.DRAW4)
        {
            deck.DrawTwoCards(GetVictim(card));
        }

        deck.DrawTwoCards(GetVictim(card));
    }

    private string GetVictim(CardSuperClass card)
    {
        if(TurnOrderIsReversed)
        {
            return card.Owner!.PreviousPlayer.Name;
        }

        return card.Owner!.NextPlayer.Name;
    }

    private bool IsCardAllowed(CardSuperClass card)
    {
        return (ActiveColour == card.ActiveColour ||
                ActiveValue == card.ActiveValue ||
                ActiveColour == Colour.WILD ||
                card.ActiveColour == Colour.WILD) &&
                CheckCurrentTurnHolder(card);
    }

    private bool CheckCurrentTurnHolder(CardSuperClass card)
    {
        return card.Owner == GetCurrentTurnHolder();
    }

    private Player GetCurrentTurnHolder()
    {
        if (TurnOrderIsReversed)
        {
            return this.Owner!.PreviousPlayer;
        }

        return this.Owner!.NextPlayer;
    }

    private bool CheckChangeTurnOrder(Value cardValue) {

        return cardValue switch
        {
            Value.REVERSE => !TurnOrderIsReversed,
            _ => TurnOrderIsReversed
        };
    }

    private Player GetNewOwner(CardSuperClass card)
    {
        return CheckSkipTurn(card);
    }

    private Player CheckSkipTurn(CardSuperClass card)
    {
        if(card.ActiveValue == Value.SKIP)
        {
            return PerformSkipTurn(card);
        }

        return card.Owner!;
    }

    private Player PerformSkipTurn(CardSuperClass card)
    {
        if (TurnOrderIsReversed)
        {
            return card.Owner!.PreviousPlayer;
        }

        return card.Owner!.NextPlayer;
    }

    public void ChangeColour(Colour colour)
    {
        this.ActiveColour = colour;
    }

    internal Player GetOwnerOfSkipTurn()
    {
        if (!TurnOrderIsReversed)
        {
            return Owner!.PreviousPlayer;
        }

        return Owner!.NextPlayer;
    }

    internal Player ChangeTurn()
    {
        if (TurnOrderIsReversed)
        {
            return Owner!.PreviousPlayer;
        }

        return Owner!.NextPlayer;
    }

    internal bool DrawIsValid(string playerName)
    {
        if (TurnOrderIsReversed)
        {
            return (playerName == Owner!.PreviousPlayer.Name);
        }

        return (playerName == Owner!.NextPlayer.Name);
    }

    internal void SetPileTopCardForStartGame(Deck deck, string[] players)
    {
        var pileTopCard = deck.GetCompleteCard(players[^1]);
        ActiveColour = pileTopCard.ActiveColour;
        ActiveValue = pileTopCard.ActiveValue;
        Owner = pileTopCard.Owner;
        Path = pileTopCard.Path;
        pileTopCard.IsPlayed = true;
    }
}
