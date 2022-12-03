using Application.Contracts.Infrastructure;
using Application.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Mail;

public class EmailSender : IEmailSender
{
    private EmailSettings settings { get; }

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        settings = emailSettings.Value; 
    }

    public async Task<bool> SendEmailAsync(Email email)
    {
        var client = new SendGridClient(settings.ApiKey);
        var to = new EmailAddress(email.To);
        var from = new EmailAddress()
        {
            Email = settings.FromAddress,
            Name = settings.FromName
        };

        var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
        var response = await client.SendEmailAsync(message);

        return response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted;
    }
}
