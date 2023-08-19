using Application.Auth.Common;
using Application.Interfaces;
using MediatR;
using OneOf;

namespace Application.Auth.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, OneOf<AuthenticationRespone, AuthenticationError>>
{
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OneOf<AuthenticationRespone, AuthenticationError>>
        Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (user is not null && user.Password == request.Password)
        {
            return new AuthenticationRespone("authenticatiedToken");
        }
        return AuthenticationError.InvalidCredentials;
    }
}
