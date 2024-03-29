﻿using MyApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MyApp.Domain;
public class Pile : CardSuperClass
{
    public bool TurnOrderIsReversed { get; set; }

    public Pile(String[] names)
    {
        this.Owner = new Player(names);
        this.ActiveColour = Colour.ALL;
        this.ActiveValue = Value.FOUR;
        this.TurnOrderIsReversed = false;
        this.Path = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/fed3bb24-454f-4bdf-a721-6aa8f23e7cef/d9gnihf-ec16caeb-ec9c-4870-9480-57c7711d844f.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2ZlZDNiYjI0LTQ1NGYtNGJkZi1hNzIxLTZhYThmMjNlN2NlZlwvZDlnbmloZi1lYzE2Y2FlYi1lYzljLTQ4NzAtOTQ4MC01N2M3NzExZDg0NGYucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.kp5EommPFQl1sDPPtC-p8JloXDTm3CyNUgoievwh8Kw";
    }

    internal void AddToPile(Deck deck, Card card)
    {
        if (IsCardAllowed(card))
        {
            ChangePileTopCard(deck, card);
            card.UpdateIsPlayed();
        }
        else if (CheckNotyourTurnException(card))
        {
            throw new NotYourTurnException();
        }
        else
        {
            throw new InvalidCardException();
        }
    }

    private bool CheckNotyourTurnException(Card card)
    {
        return (GetCurrentTurnHolder() != card.Owner);
    }

    private void ChangePileTopCard(Deck deck, Card card)
    {
        this.ActiveColour = card.ActiveColour;
        this.ActiveValue = card.ActiveValue;

        if (CardIsDrawCard(card))
        {
            DrawCorrectAmountOfCards(deck, card);
        }

        this.TurnOrderIsReversed = CheckChangeTurnOrder(card.ActiveValue);
        this.Path = card.Path;
        this.Owner = GetNewOwner(card);
    }

    private void DrawCorrectAmountOfCards(Deck deck, Card card)
    {
        if(card.ActiveValue == Value.DRAW_FOUR)
        {
            deck.DrawTwoCards(GetVictim(card));
        }

        deck.DrawTwoCards(GetVictim(card));
    }

    private string GetVictim(Card card)
    {
        if(TurnOrderIsReversed)
        {
            return card.Owner!.PreviousPlayer.Name;
        }

        return card.Owner!.NextPlayer.Name;
    }

    private bool CardIsDrawCard(Card card)
    {
        return card.ActiveValue == Value.DRAW_TWO ||
               card.ActiveValue == Value.DRAW_FOUR;
    }

    private bool IsCardAllowed(Card card)
    {
        return (this.ActiveColour == card.ActiveColour ||
                this.ActiveValue == card.ActiveValue ||
                this.ActiveColour == Colour.ALL ||
                card.ActiveColour == Colour.ALL) &&
                CheckCurrentTurnHolder(card);
    }

    private bool CheckCurrentTurnHolder(Card card)
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

    private Player GetNewOwner(Card card)
    {
        return CheckSkipTurn(card);
    }

    private Player CheckSkipTurn(Card card)
    {
        if(card.ActiveValue == Value.SKIPTURN)
        {
            return PerformSkipTurn(card);
        }

        return card.Owner!;
    }

    private Player PerformSkipTurn(Card card)
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
            return this.Owner!.PreviousPlayer;
        }

        return this.Owner!.NextPlayer;
    }

    internal Player ChangeTurn()
    {
        if (TurnOrderIsReversed)
        {
            return this.Owner!.PreviousPlayer;
        }

        return this.Owner!.NextPlayer;
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
        var pileTopCard = deck.GetCompleteCard(players[players.Length - 1]);
        this.ActiveColour = pileTopCard.ActiveColour;
        this.ActiveValue = pileTopCard.ActiveValue;
        this.Owner = pileTopCard.Owner;
        this.Path = pileTopCard.Path;
        pileTopCard.IsPlayed = true;
    }
}
