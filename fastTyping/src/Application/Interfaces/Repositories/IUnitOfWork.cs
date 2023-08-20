namespace Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IScoreRepository ScoreRepository { get; }
}
