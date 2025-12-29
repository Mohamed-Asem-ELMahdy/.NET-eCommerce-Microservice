using eCommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eCommerce.SharedLibrary.DI;

public static class SharedServiceContainer
{
    public static IServiceCollection AddSharedServices<TContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName) where TContext : DbContext
    {
        // Add Generic DbContext
        services.AddDbContext<TContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString(connectionStringName),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(TContext).Assembly.FullName)
            )
        );

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
            .CreateLogger();

        // Add JWT Authentication
        services.AddJWTAuthenticationScheme(configuration);

        return services;
    }

    public static IApplicationBuilder UseSharedMiddleware(this IApplicationBuilder app)
    {
        // Use Global Exception Handler
        app.UseMiddleware<GlobalException>();
        app.UseMiddleware<ApiGateway>();

        // Use Authentication and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
