using Microsoft.AspNetCore.Mvc;
using MyApp.Domain;
using static MyApp.Domain.Card;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PileController : ControllerBase
{

    [HttpPost()]
    public UpdatedGameState Post(PileInput input)
    {
        int count = input.Counter;
        Card card = input.Hand[input.CardIndex];
        card.PlayCard();

        Pile topCard = card.Pile;
        return new UpdatedGameState
        {
            Value = count,
            Pile = topCard,
            Hand = input.Hand.Where((card, index) => index != input.CardIndex).ToArray()
        }; 
    }
}

public class PileInput
{
    public Card[] Hand { get; set; }
    public int Counter { get; set; }
    public int CardIndex { get; set; }
}

public class UpdatedGameState
{
    public int Value { get; set; }
    public Pile? Pile { get; set; }
    public Card[]? Hand { get; set; }
}