using Application.Interfaces.Repositories;
using Domain.Models.Dtos;
using Domain.Models.Entities;
using Domain.Models.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ScoreRepository : IScoreRepository
{
    private readonly FastTypingDbContext _context;

    public ScoreRepository(FastTypingDbContext context)
    {
        _context = context;
    }

    public async Task AddScoreAsnyc(Score score)
    {
        await _context.Scores.AddAsync(score);
        await _context.SaveChangesAsync();
    }

    public Task<List<ScoreDto>> GetScoresForUserAsync(int userId)
    {
        return _context.Scores
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Id)
            .Take(100)
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed,
                x.Language
            ))
            .ToListAsync();
    }

    // TODO: Try to use AsyncEnumerable
    public Task<List<ScoreInfoDto>> GetTop100ScoresByAccuracyAsnyc()
    {
        return  _context.Scores
            .AsNoTracking()
            .OrderByDescending(x => x.Accuracy)
            .Take(100)
            .Include(x => x.User)
            .Select(x => new ScoreInfoDto(
                x.User.Username,
                x.Accuracy,
                x.Speed,
                x.Language
                ))
            .ToListAsync();
    }

    // TODO: Try to use AsyncEnumerable
    public Task<List<ScoreInfoDto>> GetTop100ScoresBySpeedAsync()
    {
        return _context.Scores
            .AsNoTracking()
            .OrderByDescending(x => x.Speed)
            .Take(100)
            .Include(x => x.User)
            .Select(x => new ScoreInfoDto(
                x.User.Username,
                x.Accuracy,
                x.Speed,
                x.Language
                ))
            .ToListAsync();
    }

    public Task<ScoreDto?> GetUserBestAccuracyScoreAsync(int userId, Language Language)
    {
        return _context.Scores
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Language == Language)
            .OrderByDescending(x => x.Accuracy)
            .Take(1)
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed,
                x.Language
                ))
            .FirstOrDefaultAsync();
    }

    public Task<ScoreDto?> GetUserBestSpeedScoreAsync(int userId, Language Language)
    {
        return _context.Scores
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Language == Language)
            .OrderByDescending(x => x.Speed)
            .Take(1)
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed,
                x.Language
                ))
            .FirstOrDefaultAsync();
    }
}
