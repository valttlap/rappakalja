using Microsoft.AspNetCore.SignalR;

namespace Sanasoppa.API.Hubs;

public class GameHub : Hub
{
    public GameHub()
    {
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
