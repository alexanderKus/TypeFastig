using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Database;
using Moq;
using Domain.Models.Entities;
using Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class ScoreRepositoryTests
{
  private readonly Mock<FastTypingDbContext> _dbContextMock;
  private readonly ScoreRepository _repo;

  public ScoreRepositoryTests()
  {
     var options = new DbContextOptionsBuilder<FastTypingDbContext>()
                      .UseInMemoryDatabase(databaseName: "TestDatabase")
                      .Options;
    _dbContextMock = new Mock<FastTypingDbContext>(options);
    _repo = new ScoreRepository(_dbContextMock.Object);
  }

  [Fact]
  public async Task Score_Should_BeAdded()
  {
      Score score = new()
      {
          Id = 0,
          Speed = 90,
          Accuracy = 79,
          Language = Language.PYTHON
      };

      await _repo.AddScoreAsnyc(score);

      _dbContextMock.Verify(context => context.Scores.Add(It.IsAny<Score>()), Times.Once());
  }
}