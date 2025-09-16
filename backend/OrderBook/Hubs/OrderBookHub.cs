using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OrderBookApi.Hubs
{
    public class OrderBookHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", "You are connected to OrderBookHub");
            await base.OnConnectedAsync();
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
        
        public async Task RequestLatestSnapshot()
        {
            await Clients.Caller.SendAsync("Info", "This would send back the latest snapshot");
        }
    }
}