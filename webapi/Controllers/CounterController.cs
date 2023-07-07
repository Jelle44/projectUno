using Microsoft.AspNetCore.Mvc;
using MyApp.Domain;
using System.Text.Json;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CounterController : ControllerBase
{ 
    static Dictionary<string, Uno> games = new Dictionary<string, Uno>();

    [HttpGet()]
    public UnoDTO NewGame()
    {
        string[] players = { "Timmy", "Jimmy" };
        //if !exists: create new game + return player1, else get game + return player 2
        if (!games.ContainsKey("password"))
        {
            Uno unoGame = new(players);
            games["password"] = unoGame;
            return new UnoDTO(unoGame, players[0]);
        } else
        {
            return new UnoDTO(games["password"], players[1]);
        }
        
        
        
    }

    [HttpPost()]
    public UnoDTO DrawCard(NameDTO playerId)
    {
        var unoGame = games["password"];
        unoGame.DrawCard(playerId.Name);
        unoGame.Counter = Uno.DecreaseCounter(unoGame.Counter);
        return new UnoDTO(unoGame, playerId.Name);
    }

    [HttpPost()]
    public UnoDTO PlayCard(PlayCardDTO card)
    {
        var unoGame = games["password"];
        Card[] playerHand = unoGame.Cards.Where(unoCard => unoCard.Owner?.Name == card.Name && !unoCard.IsPlayed).ToArray();
        Card selectedCard = playerHand[card.CardIndex];
        selectedCard.PlayCard();
        string cardOwner = selectedCard.Owner!.Name; 
        return new UnoDTO(unoGame, cardOwner);
    }

    [HttpGet]
    public IEnumerable<string> GetSessionInfo()
        //TODO no longer needed
    {
        List<string> sessionInfo = new List<string>();

        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariables.SessionKeyUsername)))
        {
            HttpContext.Session.SetString(SessionVariables.SessionKeyUsername, "Timmy");
            HttpContext.Session.SetString(SessionVariables.SessionKeySessionId, "myUno");// Guid.NewGuid().ToString());
        }

        var userName = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
        var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

        sessionInfo.Add(userName);
        sessionInfo.Add(sessionId);

        return sessionInfo;
    }
}