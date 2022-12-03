using Application.Contracts;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService authService;

    public AccountController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        var response = await authService.Login(request);

        return Ok(response);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
    {
        var response = await authService.Register(request);

        return Ok(response);
    }
}
