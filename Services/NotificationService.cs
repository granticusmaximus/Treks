using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Treks.Hubs;
using Treks.Models;

namespace Treks.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailService _emailService;
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

        public async Task NotifyUserAsync(ApplicationUser recipient, string subject, string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(recipient.Email))
                {
                    await _emailService.SendEmailAsync(recipient.Email, subject, message);
                    _logger.LogInformation("Email sent to {Email}", recipient.Email);
                }

                if (!string.IsNullOrWhiteSpace(recipient.Id))
                {
                    await _hubContext.Clients.User(recipient.Id)
                        .SendAsync("ReceiveNotification", message);
                    _logger.LogInformation("SignalR toast sent to user {UserId}", recipient.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to notify user {UserId}", recipient.Id);
            }
        }

        public async Task NotifyTicketChangedAsync(Ticket ticket)
        {
            if (ticket.AssignedUser == null)
            {
                _logger.LogWarning("Cannot notify: Ticket has no assigned user.");
                return;
            }

            string subject = $"Ticket '{ticket.Title}' Updated";
            string body = $"Hello {ticket.AssignedUser.FullName},\n\n" +
                          $"The ticket titled \"{ticket.Title}\" has been updated.\n" +
                          $"Please review the changes.\n\nThanks,\nTreks Team";

            await NotifyUserAsync(ticket.AssignedUser, subject, body);
        }

        public async Task ShowToastAsync(string message)
        {
            try
            {
                // Broadcasts to all connected users. Adjust to individual targeting as needed.
                await _hubContext.Clients.All.SendAsync("ReceiveToast", message);
                _logger.LogInformation("Global toast notification sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting toast message.");
            }
        }
    }
}