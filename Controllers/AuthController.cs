using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketSys.Services;

namespace TicketSys.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService iAuthService;
    
    public AuthController(IAuthService _AuthService)
    {
        iAuthService = _AuthService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await iAuthService.Register(request);
        if (result == null)
            return BadRequest(new { message = "User already exists" });
        
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await iAuthService.Login(request);
        if (result == null)
            return Unauthorized(new { message = "Invalid credentials" });
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("getCurrentUser")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            email = User.FindFirst(ClaimTypes.Email)?.Value,
            role = User.FindFirst(ClaimTypes.Role)?.Value,
            fullName = User.FindFirst(ClaimTypes.Name)?.Value
        });
    }
}
