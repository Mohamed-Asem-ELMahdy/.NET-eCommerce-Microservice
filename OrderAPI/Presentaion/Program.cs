using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure Services (DbContext, JWT, Serilog, IOrder, IOrderService, HttpClient, Polly)
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Use Infrastructure Middleware (Global Exception, API Gateway, Authentication, Authorization)
// Uncomment when running behind API Gateway
// app.UseInfrastructureMiddlewares();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();