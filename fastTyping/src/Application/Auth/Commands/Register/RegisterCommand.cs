using Application.Auth.Common;
using MediatR;
using OneOf;

namespace Application.Auth.Commands.Register;

public record RegisterCommand(string Username, string Password, string Email)
    : IRequest<OneOf<AuthenticationRespone, AuthenticationError>>;
