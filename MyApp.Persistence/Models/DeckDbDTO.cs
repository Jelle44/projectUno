using MyApp.Domain;

namespace MyApp.Persistence.Models;

public class DeckDbDTO
{
    public int Counter { get; }

    public int HandSizePlayerOne { get; }
    public int HandSizePlayerTwo { get; }

    public PlayerDbDTO[] Player { get; }

    public CardDbDTO PileTopCard { get; }

    public string PlayerId { get; set; }

    public bool IsGameOver { get; set; } = false;

    public DeckDbDTO(Deck uno, string playerId, string? winner)
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
        Player = new PlayerDbDTO[1];
        Player[0] = new PlayerDbDTO(uno, playerId);

        PileTopCard = new CardDbDTO(uno.Pile);

        if (winner != null)
        {
            IsGameOver = true;
        }
    }
}