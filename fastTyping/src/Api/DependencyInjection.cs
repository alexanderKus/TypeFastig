using System.Reflection;

namespace Api;

public static class DependencyInjection
{
    private static string MyAllowSpecificOrigins = "_allowAllOrigins";

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
                              });
        });
        return services;
    }
}
