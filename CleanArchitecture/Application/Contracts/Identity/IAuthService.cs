using Application.Models;


namespace Application.Contracts;


public interface IAuthService
{
    public Task<AuthResponse> Login(AuthRequest request);
    public Task<RegistrationResponse> Register(RegistrationRequest request);    
}
