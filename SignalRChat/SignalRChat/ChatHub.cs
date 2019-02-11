using Microsoft.AspNet.SignalR;
using SignalRChat.Annotations;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        [UsedImplicitly]
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }
    }
}