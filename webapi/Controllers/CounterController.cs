using Microsoft.AspNetCore.Mvc;
using MyApp.Domain;
using MyApp.Domain.Enums;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CounterController : ControllerBase
{ 
    static Dictionary<string, Game> games = new ();

    [HttpGet()]
    public DeckDTO NewGame()
    {
        string[] players = { "Timmy", "Jimmy" };
        if (!games.ContainsKey("password"))
        {
            Game unoGame = new(players);
            games["password"] = unoGame;
            return new DeckDTO(unoGame, players[0], null);
        } else
        {
            var unoGame = games["password"];
            if(unoGame.PlayerHasNoCardsInHand())
            {
                return new DeckDTO(unoGame, players[1], unoGame.Deck.Pile.Owner!.Name);
            }
            return new DeckDTO(games["password"], players[1], null);
        } 
    }

    [HttpPost()]
    public DeckDTO DrawCard(NameDTO playerId)
    {
        var unoGame = games["password"];
        unoGame.Deck.DrawCard(playerId.Name!);

        return new DeckDTO(unoGame, playerId.Name!, null);
    }

    [HttpPost()]
    public DeckDTO PlayCard(PlayCardDTO card)
    {
        var unoGame = games["password"];

        var colour = Enum.Parse<Colour>(card.Colour!);
        var value = Enum.Parse<Value>(card.Value!);

        var newColour = colour;
        if (card.NewColour != null)
        { 
            newColour = Enum.Parse<Colour>(card.NewColour.ToUpper());
        }

        var winner = unoGame.UpdateGameState(card.Name!, value, colour, newColour);
        var cardOwner = card.Name!;

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

        return new PlayerDTO(game.Deck, playerId.Name!);
    }

    //[HttpPost()]
    //public DeckDTO SaveGame()
    //{
    //    var game = games["password"];
    //    var savedGame = new DeckDTO(game);
    //    return savedGame;
    //}
}