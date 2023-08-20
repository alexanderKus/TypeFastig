namespace Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string username);
}
