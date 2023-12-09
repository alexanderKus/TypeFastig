using System.Net;
using Api.Controllers.Common;
using Application.Scores.Commands;
using Application.Scores.Common;
using Application.Scores.Queries;
using AutoMapper;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetScoreForUser(int userId, [FromQuery]Language lang)
    {
        UserScoresQuery userScoresQuery = new(userId, lang);
        OneOf<List<ScoreDto>, ScoreError> result =
            await _mediator.Send(userScoresQuery);
        return result.Match(
            scores => Ok(JsonConvert.SerializeObject(result.Value)),
            error => Problem(
                detail: result.Value.ToString(), statusCode: (int)HttpStatusCode.NotFound));
    }

    [HttpGet("GetBestSpeedScore/{userId}")]
    public async Task<IActionResult> GetBestSpeedScore(int userId, [FromQuery]Language lang)
    {
        UserBestSpeedScore userBestSpeedScoreQuery = new(userId, lang);
        OneOf<ScoreDto?, ScoreError> result =
            await _mediator.Send(userBestSpeedScoreQuery);
        return result.Match(
            score => Ok(JsonConvert.SerializeObject(result.Value)),
            error => Problem(
                detail: result.Value.ToString(), statusCode: (int)HttpStatusCode.NotFound));
    }

    [HttpGet("GetBestAccuracyScore/{userid}")]
    public async Task<IActionResult> GetBestAccuracyScore(int userId, [FromQuery]Language lang)
    {
        UserBestAccuracyScore userBestAccuracyScoreQuery = new(userId, lang);
        OneOf<ScoreDto?, ScoreError> result =
            await _mediator.Send(userBestAccuracyScoreQuery);
        return result.Match(
            score => Ok(JsonConvert.SerializeObject(result.Value)),
            error => Problem(
                detail: result.Value.ToString(), statusCode: (int)HttpStatusCode.NotFound));
    }

    [AllowAnonymous]
    [HttpGet("GetTop100Scores")]
    public async Task<IActionResult> GetTop100Scores()
    {
        Top100ScoresQuery top100ScoresQuery = new();
        Top100ScoresDto scores = await _mediator.Send(top100ScoresQuery);
        return Ok(scores);
    }
}

