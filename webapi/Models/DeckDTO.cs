using MyApp.Domain;

namespace webapi.Models;
public class DeckDTO
{
    public int Counter { get; }

    public int HandSizePlayerOne { get; }
    public int HandSizePlayerTwo { get; }

    public PlayerDTO[] Player { get; }

    public CardDTO PileTopCard { get; }

    public string PlayerId { get; set; }

    public bool IsGameOver { get; set; } = false;

    public DeckDTO(Deck uno, string playerId, string? winner)
    {
        PlayerId = playerId;
        Counter = uno.Counter;
        HandSizePlayerOne = uno.Cards.Where(card =>
                                            card.Owner?.Name == "Timmy" &&
                                            !card.IsPlayed)
                                     .ToArray()
                                     .Length;
        HandSizePlayerTwo = uno.Cards.Where(card =>
                                            card.Owner?.Name == "Jimmy" &&
                                            !card.IsPlayed)
                                     .ToArray()
                                     .Length;
        Player = new PlayerDTO[1];
        Player[0] = new PlayerDTO(uno, playerId);

        PileTopCard = new CardDTO(uno.Pile);

        if (winner != null)
        {
            IsGameOver = true;
        }
    }
}

