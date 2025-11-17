using Shared.Messaging.Infastructure;
using UserService.Contexts;
using UserService.Repository;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Repository Services (Data-stores)
builder.Services.AddDbContext<UsersContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();

// Exchange Services (Messaging)
builder.Services.AddSingleton<IMessageBrokerConnection, RabbitMQConnection>();
builder.Services.AddTransient<IMessageBrokerChannel, MessageBrokerChannel>();
builder.Services.AddScoped<IUsersExchangeService, UsersExchangeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => $"{DateTime.UtcNow}: User Service is running.");

app.Run();