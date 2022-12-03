
using MVC.Models.Account;

namespace MVC.Contracts;

public interface IAuthenticationService
{
    public Task<bool> Authenticate(string email, string password);
    public Task<bool> RegisterAsync(RegisterVM registerVM);
    public Task Logout();
}
