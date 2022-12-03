using Application.Contracts;
using Application.Models;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services;


public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly JwtSettings jwtSettings;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JwtSettings> options)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        jwtSettings = options.Value;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var authResponse = new AuthResponse();
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new Exception($"User with {request.Email} not found");

        var result = await signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (result.Succeeded == false)
            throw new Exception($"Credinetials for {request.Email} are not valid");

        var token = await GenerateTokenAsync(user);

        authResponse.Id = user.Id;
        authResponse.UserName = user.UserName;
        authResponse.Email = user.Email;
        authResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);

        return authResponse;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var existingUserName = await userManager.FindByNameAsync(request.UserName);

        if (existingUserName != null)
        {
            throw new Exception($"User with username {request.UserName} already exists");
        }

        var existingEmail = await userManager.FindByEmailAsync(request.Email);

        if (existingEmail != null)
        {
            throw new Exception($"User with username {request.Email} already exists");
        }

        var user = new ApplicationUser()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Employee");
            return new RegistrationResponse() { UserId = user.Id };
        }
        else
        {
            throw new Exception($"{result.Errors}");
        }

    }

    private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
    {

        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
        }

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        };
        claims.AddRange(userClaims);
        claims.AddRange(roleClaims);

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
            );

        return token;
    }
}
