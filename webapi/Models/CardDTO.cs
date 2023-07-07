using MyApp.Domain;
using static MyApp.Domain.Deck;
using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class CardDTO
    {
        public string? Path { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Colour ActiveColour { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Value ActiveValue { get; set; }

        public CardDTO(Pile pile)
        {
            this.Path = pile.Path;
            this.ActiveColour = pile.ActiveColour;
            this.ActiveValue = pile.ActiveValue;
        }

        public CardDTO(Card card)
        {
            this.Path = card.Path;
            this.ActiveColour = card.ActiveColour;
            this.ActiveValue = card.ActiveValue;
        }


    }
}
