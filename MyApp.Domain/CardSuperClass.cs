using System.Text.Json.Serialization;

namespace MyApp.Domain;

public abstract class CardSuperClass
{
    public string? Path { get; set; }
    public Player? Owner { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Colour ActiveColour { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Value ActiveValue { get; set; }

    public enum Colour
    {
        WILD,
        BLUE,
        GREEN,
        RED,
        YELLOW
    }
    public enum Value
    {
        ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE,
        DRAW2,
        DRAW4,
        CHANGE,
        REVERSE,
        SKIP
    }
}
