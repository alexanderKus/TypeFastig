using MediatR;

namespace Application.Scores.Commands;

public record ScoreAddCommand(int UserId, float Precision, TimeSpan Time)
    : IRequest<Unit>;
