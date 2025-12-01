using MessageService.Chat.Consumers;
using MessageService.Chat.Observers;
using MessageService.Chat.Repositories;
using MessageService.Configurations;
using MessageService.Message.Commands;
using MessageService.Message.Queries;
using MessageService.Message.Repositories;
using MessageService.User.Consumers;
using MessageService.User.Observers;
using MessageService.User.Repositories;
using Shared.Configurations;
using Shared.Middleware.CQRS;
using Shared.Middleware.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurations

builder.Services.Configure<MongoDBConfiguration>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<ChatQueueConfiguration>(builder.Configuration.GetSection("Queues:Chats"));
builder.Services.Configure<UserQueueConfiguration>(builder.Configuration.GetSection("Queues:Users"));

// Repository Services (Data-stores)

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Messaging Services

builder.Services.AddSingleton<IMessageBrokerConnection, RabbitMQConnection>();
builder.Services.AddTransient<IMessageBrokerChannel, MessageBrokerChannel>();

// builder.Services.AddTransient<>(); TODO: Publish Message Events

builder.Services.AddTransient<ChatEventHandler>();
builder.Services.AddHostedService<ChatEventConsumer>();

builder.Services.AddTransient<UserEventHandler>();
builder.Services.AddHostedService<UserEventConsumer>();

// Register Command & Query Handlers

builder.Services.Scan(scan =>
    scan
    .FromAssemblyOf<SendMessageCommand>() // Any command to find common assembly
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

builder.Services.Scan(scan =>
    scan
    .FromAssemblyOf<GetAllMessagesByChatQuery>() // Any query to find common assembly
    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => $"{DateTime.UtcNow}: Message Service is running.");


app.Run();