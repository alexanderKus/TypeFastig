using Domain.Models.Dtos;
using Domain.Models.Entities;
using Domain.Models.Enums;

namespace Application.Interfaces.Repositories;

public interface IScoreRepository
{
    Task AddScoreAsnyc(Score score);
    Task<List<ScoreDto>> GetScoresForUserAsync(int userId, Language language);
    Task<ScoreDto?> GetUserBestSpeedScoreAsync(int userId, Language language);
    Task<ScoreDto?> GetUserBestAccuracyScoreAsync(int userId, Language language);
    Task<List<ScoreInfoDto>> GetTop100ScoresBySpeedAsync();
    Task<List<ScoreInfoDto>> GetTop100ScoresByAccuracyAsnyc();
}
