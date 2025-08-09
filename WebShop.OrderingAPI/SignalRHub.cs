using Microsoft.AspNetCore.SignalR;

namespace WebShop.OrderingAPI
{
    public class SignalRHub : Hub
    {
        // This class can be used to implement SignalR methods for real-time communication
        // between the server and clients. Currently, it is empty but can be extended as needed.

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
