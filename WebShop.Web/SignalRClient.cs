using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebShop.Web
{
    public class SignalRClient
    {
        private readonly HubConnection _connection;
        public SignalRClient(string connectionString)
        {
            _connection = new HubConnectionBuilder()
                            .WithUrl(connectionString)
                            .WithAutomaticReconnect()
                            .Build();

            RegisterMethods();
        }

        public async Task StartConnection()
        {
            if (_connection.State != HubConnectionState.Connected && _connection.State != HubConnectionState.Connecting)
            {
                await _connection.StartAsync();
            }
        }

        public void RegisterMethods()
        {
            _connection.On("ReceiveMessage", (string user, string message) => {
                Console.WriteLine($"Just received message from {user} with message {message}");
            });
        }

        public async Task SendMessage(string message)
        {
            await _connection.InvokeAsync("SendMessage", "user1", message);
        }
    }
}
