using MyApp.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Persistence.Models;

public class DeckDbDTO
{
    [Key] public int DeckId { get; set; }
    public int Counter { get; }

    public List<PlayerDbDTO> PlayerList { get; }

    public CardDbDTO PileTopCard { get; }
    public CardDbDTO[] Deck { get; }

    public string PlayerId { get; set; }

    public DeckDbDTO()
    {
        //added for database-purposes
    }

    public DeckDbDTO(Game uno, string playerId)
    {
        Counter = uno.Deck.Counter;

        string[] players = uno.CreatePlayerList(playerId);
        PlayerList = new List<PlayerDbDTO>();
        foreach (var player in players)
        {
            PlayerDbDTO name = new(player);
            PlayerList.Add(name);
        }

        List<CardDbDTO> cardList = new();
        foreach(var card in uno.Deck.Cards)
        {
            CardDbDTO cardDTO = new(card);
            cardList.Add(cardDTO);
        }

        PileTopCard = new CardDbDTO(uno.Deck.Pile);
    }
}