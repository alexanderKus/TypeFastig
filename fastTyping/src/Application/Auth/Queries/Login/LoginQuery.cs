using Application.Auth.Common;
using MediatR;
using OneOf;

namespace Application.Auth.Queries.Login;

public record LoginQuery(string Username, string Password)
    : IRequest<OneOf<AuthenticationRespone, AuthenticationError>>;
