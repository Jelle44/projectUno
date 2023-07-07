using static MyApp.Domain.Deck;

namespace MyApp.Domain;

public class Uno
{ 
    public int Counter { get ; set; }

    public Pile Pile { get; }
    public Card[] Cards { get; } 
    public Uno(string[] players)
    {
        this.Counter = 108;
        //string[] players = { "Timmy" };
        this.Pile = new Pile(players);
        this.Cards = InitialiseAllCards(this.Pile);
    }

    public static int DecreaseCounter(int number)
    {
        number--;

        return number;
    }

    public Card DrawCard(string playerName)
    {
        Random rnd = new();
        int randomCard = rnd.Next(0, (this.Cards.Length - 1));
        Card drawnCard = this.Cards[randomCard];

        Player player = Pile.GetPlayerByName(playerName);

        drawnCard.Owner = player;

        //if (this.Pile.TurnOrderIsReversed)
        //{
        //    drawnCard.Owner = this.Pile.Owner!.PreviousPlayer;
        //} else
        //{
        //    drawnCard.Owner = this.Pile.Owner!.NextPlayer;
        //}
        return drawnCard;
    }

    private static Card[] InitialiseAllCards(Pile pile)
    {
        Card[] allCards = new Card[]
                {
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red0.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red1.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red3.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red4.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red5.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red6.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red7.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red8.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-reddraw2.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redreverse.png" },
                new Card(pile) {ActiveColour = Colour.RED, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-redskip.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow0.png"},
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow1.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow3.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow4.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow5.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow6.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow7.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow8.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellow9.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowdraw2.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowreverse.png" },
                new Card(pile) {ActiveColour = Colour.YELLOW, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-yellowskip.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green0.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green1.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green3.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green4.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green5.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green6.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green7.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green8.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-green9.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greendraw2.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenreverse.png" },
                new Card(pile) {ActiveColour = Colour.GREEN, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-greenskip.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.ZERO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue0.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.ONE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue1.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.THREE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue3.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue4.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.FIVE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue5.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SIX, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue6.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SEVEN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue7.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.EIGHT, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blue8.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.NINE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-red9.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.DRAW_TWO, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluedraw2.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.REVERSE, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-bluereverse.png" },
                new Card(pile) {ActiveColour = Colour.BLUE, ActiveValue = Value.SKIPTURN, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-blueskip.png" },
                new Card(pile) {ActiveColour = Colour.ALL, ActiveValue = Value.RECOLOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wildchange.png"},
                new Card(pile) {ActiveColour = Colour.ALL,ActiveValue = Value.DRAW_FOUR, Path = "http://unocardinfo.victorhomedia.com/graphics/uno_card-wilddraw4.png"}
                };
        return allCards;
    }
}
