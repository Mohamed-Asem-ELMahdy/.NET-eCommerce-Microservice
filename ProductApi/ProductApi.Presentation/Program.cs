using eCommerce.SharedLibrary.DI;

using ProductApi.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure Services (DbContext, JWT, Serilog, Repository)
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();
// Use Shared Middleware (Global Exception, API Gateway, Authentication, Authorization)
app.UseSharedMiddleware();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();