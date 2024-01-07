using System.Security.Cryptography;
using System.Text;
using Application.Auth.Common;
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using MediatR;
using OneOf;

namespace Application.Auth.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, OneOf<AuthenticationRespone, AuthenticationError>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginQueryHandler(
        IUnitOfWork unitOfWork, IJwtTokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<OneOf<AuthenticationRespone, AuthenticationError>>
        Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        using var sha256Hash = SHA256.Create();
        var passwdHash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(request.Password))?.ToString();
        if (user is not null && user.Password == passwdHash)
        {
            var token = _tokenGenerator.GenerateToken(user.Id, user.Username);
            return new AuthenticationRespone(token);
        }
        return AuthenticationError.InvalidCredentials;
    }
}
