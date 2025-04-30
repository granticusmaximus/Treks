using Treks.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Treks.Hubs; // If you're using SignalR
using System.Net.Mail;
using System.Net;

namespace Treks.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService _emailService; // Optional
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            ILogger<NotificationService> logger,
            IEmailService emailService,
            IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        public async Task NotifyTicketChangedAsync(Ticket ticket)
        {
            // Send SignalR toast notification (on-screen)
            await _hubContext.Clients.User(ticket.AssignedUserId)
                .SendAsync("ReceiveNotification", $"Ticket '{ticket.Title}' has been updated.");

            // Send Email
            if (!string.IsNullOrEmpty(ticket.AssignedUser?.Email))
            {
                var subject = $"Ticket '{ticket.Title}' has been updated";
                var body = $"Changes were made to ticket '{ticket.Title}'. Please review the updates.";
                await _emailService.SendEmailAsync(ticket.AssignedUser.Email, subject, body);
            }

            _logger.LogInformation($"Notification sent for ticket '{ticket.TicketId}'");
        }
    }
}