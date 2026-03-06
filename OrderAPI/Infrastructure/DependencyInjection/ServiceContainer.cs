using Application.Interfaces;
using Application.Services;
using eCommerce.SharedLibrary.DI;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;

namespace Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add Shared Services: DbContext (PostgreSQL), JWT Authentication, Serilog
        services.AddInfrastructureServices<AppDbContext>(configuration, "DefaultConnection");

        // Register Order Repository
        services.AddScoped<IOrder, OrderRepository>();

        // Register Order Service
        services.AddScoped<IOrderService, OrderService>();

        // Register HttpClient for OrderService (used to call Product & Auth services)
        services.AddHttpClient<IOrderService, OrderService>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiGateway:BaseUrl"]
                                         ?? "http://localhost:5000");
            client.Timeout = TimeSpan.FromSeconds(1);
        });

        // Register Polly Resilience Pipeline (retry with exponential back-off)
        services.AddResiliencePipeline("my-pipline", pipelineBuilder =>
        {
            pipelineBuilder.AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            });

            pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(5));
        });

        return services;
    }

    public static IApplicationBuilder UseInfrastructureMiddlewares(this IApplicationBuilder app)
    {
        // Use Shared Middleware: Global Exception Handler, API Gateway check, Auth
        app.UseSharedMiddleware();
        return app;
    }
}
