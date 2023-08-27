using MediatR;

namespace Application.Scores.Commands;

public record ScoreAddCommand(int UserId, float Accuracy, int Speed)
    : IRequest<Unit>;
