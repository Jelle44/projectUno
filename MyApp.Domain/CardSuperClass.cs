using System.Text.Json.Serialization;
using MyApp.Domain.Enums;

namespace MyApp.Domain;

public abstract class CardSuperClass
{
    public string? Path { get; set; }
    public Player? Owner { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Colour ActiveColour { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Value ActiveValue { get; set; }
}
