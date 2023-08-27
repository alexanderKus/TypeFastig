using System;
using Application.Interfaces.Repositories;
using Application.Scores.Common;
using Domain.Models.Dtos;
using Domain.Models.Entities;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public class UserBestAccuracyScoreHandler
    : IRequestHandler<UserBestAccuracyScore, OneOf<ScoreDto?, ScoreError>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserBestAccuracyScoreHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<ScoreDto?, ScoreError>> Handle(
        UserBestAccuracyScore request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
        if (user is null)
        {
            return ScoreError.DoNotExists; // TODO: UserError.DoNotExists ???
        }
        ScoreDto? highestAccuracyScore =
            await _unitOfWork.ScoreRepository.GetUserBestAccuracyScoreAsync(request.UserId);
        return highestAccuracyScore;
    }
}


