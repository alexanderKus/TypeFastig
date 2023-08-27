using Application.Interfaces.Repositories;
using Application.Scores.Common;
using Domain.Models.Dtos;
using Domain.Models.Entities;
using MediatR;
using OneOf;

namespace Application.Scores.Queries;

public class UserScoresQueryHandler
    : IRequestHandler<UserScoresQuery, OneOf<List<ScoreDto>, ScoreError>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserScoresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<List<ScoreDto>, ScoreError>> Handle(
        UserScoresQuery request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
        if (user is null)
        {
            return ScoreError.DoNotExists; // TODO: propabily UserError.DoNotExists...
        }
        return await _unitOfWork.ScoreRepository.GetScoresForUserAsync(request.UserId);
    }
}
