using MyApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Persistence.Models;
public class PlayerDbDTO
{
        public string Name { get; set; }
        public CardDbDTO[] Hand { get; }

        public PlayerDbDTO(Deck uno, string name)
        {
            this.Name = name;

            this.Hand = uno.Cards
                .Where(card => card.Owner?.Name == name && !card.IsPlayed)
                .Select(card => new CardDbDTO(card))
                .ToArray();
        }
}
