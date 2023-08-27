using Application.Interfaces.Repositories;
using Domain.Models.Entities;
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

    public async Task<User> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _context.Users.SingleOrDefaultAsync(x => x.Email == email);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _context.Users.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _context.Users.SingleOrDefaultAsync(x => x.Username == username);
    }
}

