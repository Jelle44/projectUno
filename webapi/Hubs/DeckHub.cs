using Microsoft.AspNetCore.SignalR;

namespace webapi.Hubs;

public class DeckHub : Hub
{
    public async Task DrawCard(int newNumCards, object updatedPile) =>
       await Clients.All.SendAsync("requestReceived", newNumCards, updatedPile);

}

