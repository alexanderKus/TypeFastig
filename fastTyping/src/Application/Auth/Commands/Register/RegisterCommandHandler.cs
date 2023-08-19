using Application.Auth.Common;
using Application.Interfaces;
using Domain.Models.Common;
using MediatR;
using OneOf;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, OneOf<AuthenticationRespone, AuthenticationError>>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OneOf<AuthenticationRespone, AuthenticationError>>
        Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userRepository.GetUserByEmailAsync(request.Email);
        var userByUsername = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (userByEmail is not null)
        {
            return AuthenticationError.EmailTaken;
        }
        else if (userByUsername is not null)
        {
            return AuthenticationError.NameTaken;
        }
        User user = new() {
            Id = 0,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };
        user = await _userRepository.AddUserAsync(user);
        return new AuthenticationRespone("AuthenticatedResponse");
    }
}
