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

// Use Infrastructure Middleware (Global Exception, API Gateway, Authentication, Authorization)
// This enforces API Gateway-only access for all endpoints
//app.UseInfrastructureMiddlewares();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();