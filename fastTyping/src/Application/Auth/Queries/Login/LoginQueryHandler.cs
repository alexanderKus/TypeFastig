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
        if (user is not null && user.Password == request.Password)
        {
            var token = _tokenGenerator.GenerateToken(user.Id, user.Username);
            return new AuthenticationRespone(token);
        }
        return AuthenticationError.InvalidCredentials;
    }
}
