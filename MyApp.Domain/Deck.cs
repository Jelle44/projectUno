using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static MyApp.Domain.Pile;

namespace MyApp.Domain;

    public abstract class Deck
    {
    public string? Path { get; set; }
    public Player? Owner { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Colour ActiveColour { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Value ActiveValue { get; set; }

    public enum Colour
    {
        ALL,
        BLUE,
        GREEN,
        RED,
        YELLOW
    }
    public enum Value
    {
        ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE,
        DRAW_TWO,
        DRAW_FOUR,
        RECOLOUR,
        REVERSE,
        SKIPTURN
    }
}
