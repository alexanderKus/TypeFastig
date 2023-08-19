using Domain.Models.Common;

namespace Application.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetUserByUsernameAsync(string Username);
	}
}
