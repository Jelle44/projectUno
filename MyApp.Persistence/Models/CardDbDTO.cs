using MyApp.Domain;
using static MyApp.Domain.CardSuperClass;
using System.Text.Json.Serialization;

namespace MyApp.Persistence.Models;
public class CardDbDTO
{
    public string? Path { get; set; }
    public string? Name { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Colour ActiveColour { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Value ActiveValue { get; set; }

    public CardDbDTO(Pile pile)
    {
        this.Name = pile.Owner!.Name;
        this.Path = pile.Path;
        this.ActiveColour = pile.ActiveColour;
        this.ActiveValue = pile.ActiveValue;
    }

    public CardDbDTO(Card card)
    {
        this.Name = card.Owner!.Name;
        this.Path = card.Path;
        this.ActiveColour = card.ActiveColour;
        this.ActiveValue = card.ActiveValue;
    }
}