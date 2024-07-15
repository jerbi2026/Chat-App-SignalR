using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Backend_Chat.Hubs
{
    public class ChatHub : Hub
    {
        /* public override async Task OnConnectedAsync()
         {
             Console.WriteLine($"User connected: {Context.ConnectionId}");
             await base.OnConnectedAsync();
         }

         public override async Task OnDisconnectedAsync(Exception? exception)
         {
             Console.WriteLine($"User disconnected: {Context.ConnectionId}");
             await base.OnDisconnectedAsync(exception);
         }

         public async Task SendMessage(string user, string message)
         {
             Console.WriteLine($"SendMessage called with user: {user}, message: {message}");
             await Clients.All.SendAsync("ReceiveMessage", user, message);
         }*/

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"User connected: {Context.ConnectionId}");
            ConnectedUsers.myConnectedUsers.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User disconnected: {Context.ConnectionId}");
            ConnectedUsers.myConnectedUsers.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"SendMessage called with user: {user}, message: {message}");
            if (string.IsNullOrEmpty(user))
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            else
                await Clients.Client(user).SendAsync("ReceiveMessage", user, message);
        }
    }
}

