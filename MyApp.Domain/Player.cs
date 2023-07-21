using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Domain;
public class Player
{
    public Player NextPlayer { get; set; }
    public Player PreviousPlayer { get; set; }
    public string Name { get; set; }
    public bool Uno { get; set; }

    public Player(string[] names)
    {
        int numPlayersToMake = names.Length - 1;
        if (numPlayersToMake > 0)
        {
            this.Name = names[numPlayersToMake];
            this.PreviousPlayer = this;
            this.Uno = false;
            this.NextPlayer = new Player(names, (numPlayersToMake - 1), this, this);
        }
        else
        {
            this.Name = names[0];
            this.NextPlayer = this;
            this.PreviousPlayer = this;
            this.Uno = false;
        }
    }

    public Player(string[] names, int numPlayersToMake, Player playerOne, Player prevPlayer)
    {
        if(numPlayersToMake > 0)
        {
            this.Name = names[numPlayersToMake];
            this.PreviousPlayer = prevPlayer;
            this.Uno = false;
            this.NextPlayer = new Player(names, (numPlayersToMake - 1), playerOne, this);
        }
        else
        {
            this.Name = names[numPlayersToMake];
            this.NextPlayer = playerOne;
            this.PreviousPlayer = prevPlayer;
            this.Uno = false;
            NextPlayer.PreviousPlayer = this;
        }
    }

    public Player GetPlayerByName(string playerName)
    {
        if (Name == playerName) {
            return this;
        } else
        {
            return NextPlayer.GetPlayerByName(playerName);
        }
    }

    public void UpdateUno()
    {
        this.Uno = !this.Uno;
    }
}
