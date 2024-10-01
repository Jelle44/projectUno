namespace MyApp.Domain;
public class Player
{
    public Player NextPlayer { get; set; }
    public Player PreviousPlayer { get; set; }
    public string Name { get; set; }
    public bool Uno { get; set; }

    public Player(IReadOnlyList<string> names)
    {
        var numPlayersToCreate = names.Count - 1;
        if (numPlayersToCreate > 0)
        {
            Name = names[numPlayersToCreate];
            PreviousPlayer = this;
            Uno = false;
            NextPlayer = new Player(names, (numPlayersToCreate - 1), this, this);
        }
        else
        {
            Name = names[0];
            NextPlayer = this;
            PreviousPlayer = this;
            Uno = false;
        }
    }

    public Player(IReadOnlyList<string> names, int numPlayersToCreate, Player playerOne, Player prevPlayer)
    {
        if (numPlayersToCreate > 0)
        {
            Name = names[numPlayersToCreate];
            PreviousPlayer = prevPlayer;
            Uno = false;
            NextPlayer = new Player(names, (numPlayersToCreate - 1), playerOne, this);
        }
        else
        {
            Name = names[numPlayersToCreate];
            NextPlayer = playerOne;
            PreviousPlayer = prevPlayer;
            Uno = false;
            NextPlayer.PreviousPlayer = this;
        }
    }

    public Player GetPlayerByName(string playerName)
    {
        if (Name == playerName)
        {
            return this;
        }

        return NextPlayer.GetPlayerByName(playerName);
    }

    public void UpdateUno()
    {
        Uno = !Uno;
    }
}
