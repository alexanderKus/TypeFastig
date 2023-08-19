using Application.Interfaces;
using Domain.Models.Common;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FastTypingDbContext _context;

    public UserRepository(FastTypingDbContext context)
	{
        _context = context;
	}

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        return user;
    }
}

