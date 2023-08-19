using Application.Auth.Common;
using MediatR;
using OneOf;

namespace Application.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, OneOf<AuthenticationRespone, AuthenticationError>>
{
    public RegisterCommandHandler()
    {
    }

    public async Task<OneOf<AuthenticationRespone, AuthenticationError>>
        Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return AuthenticationError.EmailTaken;
    }
}
