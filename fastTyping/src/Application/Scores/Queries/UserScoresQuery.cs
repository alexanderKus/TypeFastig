using Application.Scores.Common;
using Domain.Models.Dtos;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public record UserScoresQuery(int UserId)
    : IRequest<OneOf<List<ScoreDto>, ScoreError>>;
