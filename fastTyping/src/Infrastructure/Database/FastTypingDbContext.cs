using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class FastTypingDbContext : DbContext
    {
        public FastTypingDbContext(DbContextOptions<FastTypingDbContext> options)
			: base (options)
		{
		}

        public DbSet<User> Users { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}
