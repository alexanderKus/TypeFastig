using Application.Interfaces.Repositories;
using Domain.Models.Dtos;
using Domain.Models.Entities;
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
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed
            ))
            .ToListAsync();
    }

    public Task<ScoreDto?> GetUserBestAccuracyScoreAsync(int userId)
    {
        return _context.Scores
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Accuracy)
            .Take(1)
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed
                ))
            .FirstOrDefaultAsync();
    }

    public Task<ScoreDto?> GetUserBestSpeedScoreAsync(int userId)
    {
        return _context.Scores
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Speed)
            .Take(1)
            .Select(x => new ScoreDto(
                x.Id,
                x.Accuracy,
                x.Speed
                ))
            .FirstOrDefaultAsync();
    }
}
