using System;
using Application.Interfaces.Repositories;
using Application.Scores.Common;
using Domain.Models.Dtos;
using Domain.Models.Entities;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public class UserBestSpeedScoreHandler :
    IRequestHandler<UserBestSpeedScore, OneOf<ScoreDto?, ScoreError>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserBestSpeedScoreHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<ScoreDto?, ScoreError>> Handle(
        UserBestSpeedScore request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
        if (user is null)
        {
            return ScoreError.DoNotExists; // TODO: or userError.DoNotExists
        }
        ScoreDto? highestSpeedScore =
            await _unitOfWork.ScoreRepository.GetUserBestSpeedScoreAsync(request.UserId, request.Language);
        return highestSpeedScore;
    }
}
