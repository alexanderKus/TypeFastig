using System.Net;
using Application.Auth.Common;
using Application.Auth.Queries.Login;
using Domain.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        OneOf<AuthenticationRespone, AuthenticationError> token =
            await _mediator.Send(new LoginQuery(loginRequest.Username, loginRequest.Password));
        return token.Match(
            authenticationRespone => Ok(token),
            authenticationError => Problem(detail: token.ToString(), statusCode: (int)HttpStatusCode.Conflict));
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] RegisterRequest registerRequest)
    {
        return Ok();
    }
}
