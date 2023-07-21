using Microsoft.AspNetCore.Mvc;
using MyApp.Domain;
using System.Text.Json;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CounterController : ControllerBase
{ 
    static Dictionary<string, Deck> games = new Dictionary<string, Deck>();

    [HttpGet()]
    public DeckDTO NewGame()
    {
        string[] players = { "Timmy", "Jimmy" };
        if (!games.ContainsKey("password"))
        {
            Deck unoGame = new(players);
            games["password"] = unoGame;
            return new DeckDTO(unoGame, players[0], null);
        } else
        {
            var unoGame = games["password"];
            if(unoGame.PlayerHasNoCardsInHand())
            {
                return new DeckDTO(unoGame, players[1], unoGame.Pile.Owner!.Name);
            }
            return new DeckDTO(games["password"], players[1], null);
        } 
    }

    [HttpPost()]
    public DeckDTO DrawCard(NameDTO playerId)
    {
        var unoGame = games["password"];
        unoGame.DrawCard(playerId.Name!);

        return new DeckDTO(unoGame, playerId.Name!, null);
    }

    [HttpPost()]
    public DeckDTO PlayCard(PlayCardDTO card)
    {
        var unoGame = games["password"];

        CardSuperClass.Colour colour = Enum.Parse<CardSuperClass.Colour>(card.Colour!);
        CardSuperClass.Value value = Enum.Parse<CardSuperClass.Value>(card.Value!);

        CardSuperClass.Colour newColour = colour;
        if (card.NewColour != null)
        { 
            newColour = Enum.Parse<CardSuperClass.Colour>(card.NewColour.ToUpper());
        }

        string? winner = unoGame.UpdateGameState(card.Name!, value, colour, newColour);
        string cardOwner = card.Name!;

        return new DeckDTO(unoGame, cardOwner, winner);
    }

    [HttpPost()]
    public DeckDTO SignalUno(NameDTO playerId)
    {
        var game = games["password"];
        game.UnoButtonWasPressed(playerId.Name!);

        return new DeckDTO(game, playerId.Name!, null);
    }

    [HttpPost()]
    public PlayerDTO ReloadHand(NameDTO playerId)
    {
        var game = games["password"];

        return new PlayerDTO(game, playerId.Name!);
    }
}