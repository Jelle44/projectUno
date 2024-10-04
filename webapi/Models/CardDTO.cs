using MyApp.Domain;
using MyApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class CardDTO
    {
        public string? Path { get; set; }
        public string? Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Colour ActiveColour { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Value ActiveValue { get; set; }

        public CardDTO(Pile pile)
        {
            this.Name = pile.Owner!.Name;
            this.Path = pile.Path;
            this.ActiveColour = pile.ActiveColour;
            this.ActiveValue = pile.ActiveValue;
        }

        public CardDTO(Card card)
        {
            this.Name = card.Owner!.Name;
            this.Path = card.Path;
            this.ActiveColour = card.ActiveColour;
            this.ActiveValue = card.ActiveValue;
        }
    }
}
