using Domain.Models.Common;

namespace Application.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetUserByEmailAsync(string email);
		Task<User?> GetUserByUsernameAsync(string Username);
		Task<User> AddUserAsync(User user);
	}
}
