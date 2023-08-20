using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{

    public UnitOfWork(
        IUserRepository userRepository, IScoreRepository scoreRepository)
    {
        UserRepository = userRepository;
        ScoreRepository = scoreRepository;
    }

    public IUserRepository UserRepository { get; }
    public IScoreRepository ScoreRepository { get; }
}
