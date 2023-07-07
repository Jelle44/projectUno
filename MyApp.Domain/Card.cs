using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static MyApp.Domain.Pile;

namespace MyApp.Domain
{
    public class Card : Deck
    {
        public bool IsPlayed { get; set; }
        public Pile Pile { get; private set; }
        public Card(Pile pile)
        {
            this.Owner = null;
            this.Pile = pile;
            this.ActiveColour = Colour.BLUE;
            this.ActiveValue = Value.ZERO;
            this.IsPlayed = false;
            this.Path = "";
        }

        public void PlayCard()
        {
            this.Pile.AddToPile(this);
            this.IsPlayed = true;
        }
    }
}
