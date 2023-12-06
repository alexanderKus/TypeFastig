using Domain.Models.Enums;
using MediatR;

namespace Application.Scores.Commands;

public record ScoreAddCommand(int UserId, float Accuracy, int Speed, Language Language)
    : IRequest<Unit>;
