using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            var users = new string[] { message.ToUserId, message.UserID };
            //await Clients.Users(users).SendAsync("receiveMessage", message);

            await Clients.All.SendAsync("receiveMessage", message);

        }
    }
}
