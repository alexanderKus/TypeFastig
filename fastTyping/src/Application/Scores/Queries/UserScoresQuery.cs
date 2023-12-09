using Application.Scores.Common;
using Domain.Models.Dtos;
using Domain.Models.Enums;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public record UserScoresQuery(int UserId, Language Language)
    : IRequest<OneOf<List<ScoreDto>, ScoreError>>;
