using eCommerce.SharedLibrary.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection;

/// <summary>
/// Service container for registering Infrastructure layer dependencies.
/// </summary>
public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Shared Services (DbContext with Postgres, JWT Authentication, Serilog)
        services.AddInfrastructureServices<AppDbContext>(configuration, "DefaultConnection");

        // Register Repository
        services.AddScoped<IProduct, ProductRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructureMiddlewares(this IApplicationBuilder app)
    {
        // Use Shared Middleware
        // shared Authentication 
        // listen only to API Gateway
        
        app.UseSharedMiddleware();
        return app;
    }
}
