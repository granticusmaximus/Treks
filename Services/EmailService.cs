using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("grant@gwsapp.net", "Grant" + " " + "Watson"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        await _smtpClient.SendMailAsync(mailMessage);
    }
}
