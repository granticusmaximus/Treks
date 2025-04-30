using System.Threading.Tasks;
using Treks.Models;

namespace Treks.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(ApplicationUser recipient, string subject, string message);
        Task NotifyTicketChangedAsync(Ticket ticket);
        Task ShowToastAsync(string message);
    }
}