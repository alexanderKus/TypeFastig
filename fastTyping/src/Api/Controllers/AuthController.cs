using System.Net;
using Application.Auth.Commands.Register;
using Application.Auth.Common;
using Application.Auth.Queries.Login;
using AutoMapper;
using Domain.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneOf;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginQuery = _mapper.Map<LoginQuery>(loginRequest);
        OneOf<AuthenticationRespone, AuthenticationError> token =
            await _mediator.Send(loginQuery);
        return token.Match(
            authenticationRespone => Ok(JsonConvert.SerializeObject(token.Value)),
            authenticationError => Problem(
                detail: token.Value.ToString(), statusCode: (int)HttpStatusCode.Conflict));
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var registerCommand = _mapper.Map<RegisterCommand>(registerRequest);
        OneOf<AuthenticationRespone, AuthenticationError> token =
            await _mediator.Send(registerCommand);
        return token.Match(
            authenticationRespone => Ok(JsonConvert.SerializeObject(token.Value)),
            authenticationError => Problem(
                detail: token.Value.ToString(), statusCode: (int)HttpStatusCode.Conflict));
    }
}
