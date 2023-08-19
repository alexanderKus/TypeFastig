﻿using Domain.Models.Common;
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
    }
}
