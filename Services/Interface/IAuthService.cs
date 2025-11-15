namespace TicketSys.Services;

public interface IAuthService
{
    Task<AuthResponse?> Register(RegisterRequest request);
    Task<AuthResponse?> Login(LoginRequest request);
}
