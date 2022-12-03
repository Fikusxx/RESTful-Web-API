using AutoMapper;
using CleanArchitecture.UI.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Contracts;
using MVC.Models.Account;
using MVC.Services.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.Services;

public class AuthenticationService : BaseHttpService, MVC.Contracts.IAuthenticationService
{
    private readonly IHttpContextAccessor contextAccessor;
    private JwtSecurityTokenHandler tokenHandler;
    private readonly IMapper mapper;

    public AuthenticationService(IClient client,
        ILocalStorageService storage,
        IHttpContextAccessor contextAccessor,
        IMapper mapper) : base(storage, client)
    {
        this.contextAccessor = contextAccessor;
        this.tokenHandler = new JwtSecurityTokenHandler();
        this.mapper = mapper;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        try
        {
            var authRequest = new AuthRequest() { Email = email, Password = password };
            var authResponse = await client.LoginAsync(authRequest);

            if (authResponse.Token != string.Empty)
            {
                var token = tokenHandler.ReadJwtToken(authResponse.Token);
                var claims = token.Claims.ToList();
                claims.Add(new Claim(ClaimTypes.Name, token.Subject));

                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                var login = contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                storage.SetStorageValue("token", authResponse.Token);

                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task Logout()
    {
        storage.ClearStorage(new List<string>() { "token" });
        await contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> RegisterAsync(RegisterVM registerVM)
    {
        var request = mapper.Map<RegistrationRequest>(registerVM);

        var response = await client.RegisterAsync(request);

        if (string.IsNullOrEmpty(response.UserId))
            return false;

        await Authenticate(registerVM.Email, registerVM.Password);

        return true;
    }
}
