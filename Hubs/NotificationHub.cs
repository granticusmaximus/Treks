using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Treks.Hubs
{
    public class NotificationHub : Hub
    {
        // Called by server to notify specific user (by user ID) about a ticket change
        public async Task SendTicketChangedAsync(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveTicketNotification", message);
        }
    }
}