using MyApp.Domain;
using static MyApp.Domain.CardSuperClass;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Persistence.Models;
public class CardDbDTO
{
    [Key] public int CardId { get; set; }
    public string? Name { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Colour ActiveColour { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Value ActiveValue { get; set; }
    public bool IsPlayed { get; set; }

    public CardDbDTO()
    {
        //added for database-purposes
    }

    public CardDbDTO(Pile pile)
    {
        this.Name = pile.Owner!.Name;
        this.ActiveColour = pile.ActiveColour;
        this.ActiveValue = pile.ActiveValue;
        this.IsPlayed = true;
    }

    public CardDbDTO(Card card)
    {
        this.Name = card.Owner?.Name;
        this.ActiveColour = card.ActiveColour;
        this.ActiveValue = card.ActiveValue;
        this.IsPlayed = card.IsPlayed;
    }
}