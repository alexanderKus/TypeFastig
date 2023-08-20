using Application.Auth.Common;
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Domain.Models.Common;
using MediatR;
using OneOf;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, OneOf<AuthenticationRespone, AuthenticationError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
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
        var token = _tokenGenerator.GenerateToken(user.Id, user.Username);
        return new AuthenticationRespone(token);
    }
}
