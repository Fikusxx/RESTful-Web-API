using Application.Models;


namespace Application.Contracts.Infrastructure;


public interface IEmailSender
{
    public Task<bool> SendEmailAsync(Email email);
}
