using MessageService.Messaging.Consumers;
using MessageService.Repositories;
using MessageService.Services;
using Shared.Messaging.Infastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessagesService, MessagesService>();

builder.Services.AddSingleton<IUserRepository, UserRepository>();

// Exchange Services (Messaging)

builder.Services.AddSingleton<IMessageBrokerConnection, RabbitMQConnection>();
builder.Services.AddTransient<IMessageBrokerChannel, MessageBrokerChannel>();
builder.Services.AddHostedService<UsersConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => $"{DateTime.UtcNow}: Message Service is running.");

app.Run();