using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketSys.Data;
using TicketSys.Models;

namespace TicketSys.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext dbContext;
    private readonly IConfiguration configure;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        dbContext = context;  
        configure = config;    
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == request.Email))
            return null;

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User",
            FullName = request.Username,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return GenerateToken(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return null;

        return GenerateToken(user);
    }

    private AuthResponse GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configure["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.FullName ?? user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: configure["Jwt:Issuer"],
            audience: configure["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(configure["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email,
            Role = user.Role,
        };
    }
}
