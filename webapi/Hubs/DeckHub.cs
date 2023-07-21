using Microsoft.AspNetCore.SignalR;

namespace webapi.Hubs;

public class DeckHub : Hub
{
    public async Task UpdateBoard(object updatedPile, int newNumCards, int p1HandSize, int p2HandSize, bool isGameOver) =>
       await Clients.All.SendAsync("requestReceived", updatedPile, newNumCards, p1HandSize, p2HandSize, isGameOver);

    public async Task UnoButtonPressed() =>
        await Clients.All.SendAsync("unoButtonWasPressed");
}

