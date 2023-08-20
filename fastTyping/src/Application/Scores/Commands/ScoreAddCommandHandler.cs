using Application.Interfaces.Repositories;
using Domain.Models.Entities;
using MediatR;

namespace Application.Scores.Commands;

public class ScoreAddCommandHandler
    : IRequestHandler<ScoreAddCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public ScoreAddCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        ScoreAddCommand request, CancellationToken cancellationToken)
    {
        Score score = new() {
            Id = 0,
            UserId = request.UserId,
            Precision = request.Precision,
            Time = request.Time
        };
        await _unitOfWork.ScoreRepository.AddScoreAsnyc(score);
        return Unit.Value;
    }
}
