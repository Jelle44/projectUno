using MyApp.Domain;

namespace webapi.Models;
public class UnoDTO
{
    
    public int Counter { get; }

    public PlayerDTO[] Player { get; }

    public CardDTO PileTopCard { get; }

    public string PlayerId { get; set; }

    public UnoDTO(Uno uno, string playerId)
    {
        PlayerId = playerId;
        Counter = uno.Counter;
        Player = new PlayerDTO[1];
        Player[0] = new PlayerDTO(uno, playerId);

        PileTopCard = new CardDTO(uno.Pile);
    }
}

