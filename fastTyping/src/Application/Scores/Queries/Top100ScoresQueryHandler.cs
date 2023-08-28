using System;
using Application.Interfaces.Repositories;
using Domain.Models.Dtos;
using MediatR;

namespace Application.Scores.Queries;

public class Top100ScoresQueryHandler
    : IRequestHandler<Top100ScoresQuery, Top100ScoresDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public Top100ScoresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Top100ScoresDto> Handle(
        Top100ScoresQuery request, CancellationToken cancellationToken)
    {
        List<ScoreInfoDto> byAccuracy = 
            await _unitOfWork.ScoreRepository.GetTop100ScoresByAccuracyAsnyc();
        List<ScoreInfoDto> bySpeed = 
            await _unitOfWork.ScoreRepository.GetTop100ScoresBySpeedAsync();
        Top100ScoresDto scores = new(
            BestAccuracy: byAccuracy ?? new List<ScoreInfoDto>(), 
            BestSpeed: bySpeed ?? new List<ScoreInfoDto>());

        return scores;
    }
}
