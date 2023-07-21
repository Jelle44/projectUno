using MyApp.Domain;

namespace webapi.Models
{
    public class PlayerDTO
    {
        public string Name { get; set; }
        public CardDTO[] Hand { get; }

        public PlayerDTO(Deck uno, string name)
        {
            this.Name = name;

            this.Hand = uno.Cards
                .Where(card => card.Owner?.Name == name && !card.IsPlayed)
                .Select(card => new CardDTO(card))
                .ToArray();
        }
    }
}
