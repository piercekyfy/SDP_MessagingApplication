using ChatService.Chat.Commands;
using ChatService.Chat.Contexts;
using ChatService.Chat.Publishers;
using ChatService.Chat.Queries;
using ChatService.Chat.Repositories;
using ChatService.Chat.WorkUnits;
using ChatService.Configurations;
using ChatService.User.Contexts;
using ChatService.User.Repositories;
using MessageService.Configurations;
using MessageService.Services.Events;
using MessageService.User.Consumers;
using Shared.Configurations;
using Shared.Middleware.CQRS;
using Shared.Middleware.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurations

builder.Services.Configure<PostgresSQLConfiguration>(builder.Configuration.GetSection("PostgresSQL"));
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<ChatExchangeConfiguration>(builder.Configuration.GetSection("Exchanges:Chats"));
builder.Services.Configure<UsersQueueConfiguration>(builder.Configuration.GetSection("Queues:Users"));

// Repository Services (Data-stores)
builder.Services.AddDbContext<ChatContext>();
builder.Services.AddDbContext<UsersContext>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IChatWorkUnit, ChatWorkUnit>();

// Messaging Services

builder.Services.AddSingleton<IMessageBrokerConnection, RabbitMQConnection>();
builder.Services.AddTransient<IMessageBrokerChannel, MessageBrokerChannel>();

builder.Services.AddTransient<ChatMessagePublisher>();

builder.Services.AddTransient<UserEventHandler>();
builder.Services.AddHostedService<UserEventConsumer>();

// Register Command & Query Handlers

builder.Services.Scan(scan =>
    scan
    .FromAssemblyOf<CreateChatCommand>() // Any command to find common assembly
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

builder.Services.Scan(scan =>
    scan
    .FromAssemblyOf<GetAllChatsQuery>() // Any query to find common assembly
    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

var app = builder.Build();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => $"{DateTime.UtcNow}: Chat Service is running.");

app.Run();