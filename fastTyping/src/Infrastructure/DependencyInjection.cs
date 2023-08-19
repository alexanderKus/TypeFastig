using Application.Interfaces;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services, IConfiguration configuration)
	{
        var conn = configuration.GetConnectionString("Default");
        services.AddDbContext<FastTypingDbContext>(options => 
        {
            options.UseNpgsql(conn);
        });
        services.AddScoped<IUserRepository, UserRepository>();
		return services;
	}
}
