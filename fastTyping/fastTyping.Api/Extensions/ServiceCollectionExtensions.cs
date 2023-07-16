using fastTyping.Common.Interfaces;
using fastTyping.Infrastructure.Services;
using Microsoft.OpenApi.Models;

namespace fastTyping.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomManager, RoomManager>();
            services.AddScoped<IContestManager, ContestManager>();
        }

        public static void AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FastTyping API v1", Version = "v1" });
            });
        }
        public static void AddCustomCors(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowALl", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                //TODO: Add origin
            });
        }
    }
}

