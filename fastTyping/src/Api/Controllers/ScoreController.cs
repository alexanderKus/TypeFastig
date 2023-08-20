using System.Net;
using Api.Controllers.Common;
using Application.Scores.Commands;
using Application.Scores.Common;
using Application.Scores.Queries;
using AutoMapper;
using Domain.Models;
using Domain.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneOf;

namespace Api.Controllers;

public class ScoreController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ScoreController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("AddScore")]
    public async Task<IActionResult> AddScore([FromBody] ScoreRequest scoreRequest)
    {
        var scoreAddCommand = _mapper.Map<ScoreAddCommand>(scoreRequest);
        await _mediator.Send(scoreAddCommand);
        return Ok();
    }

    [HttpGet("GetScoreForUser/{userId}")]
    public async Task<IActionResult> GetScoreForUser(int userId)
    {
        UserScoresQuery userScoresQuery = new(userId);
        OneOf<List<ScoreDto>, ScoreError> result =
            await _mediator.Send(userScoresQuery);
        return result.Match(
            scores => Ok(JsonConvert.SerializeObject(result.Value)),
            error => Problem(
                detail: result.Value.ToString(), statusCode: (int)HttpStatusCode.NotFound));
    }
}

