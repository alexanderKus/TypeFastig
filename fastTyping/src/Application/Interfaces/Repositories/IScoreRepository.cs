using Domain.Models.Dtos;
using Domain.Models.Entities;

namespace Application.Interfaces.Repositories;

public interface IScoreRepository
{
    Task AddScoreAsnyc(Score score);
    Task<List<ScoreDto>> GetScoresForUserAsync(int userId);
    Task<ScoreDto?> GetUserBestSpeedScoreAsync(int userId);
    Task<ScoreDto?> GetUserBestAccuracyScoreAsync(int userId);
}
