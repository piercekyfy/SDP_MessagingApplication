using ChatService.Contexts;
using ChatService.Repositories;
using UserService.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Repository Services (Data-stores)
builder.Services.AddDbContext<ChatsContext>();
builder.Services.AddDbContext<UsersContext>();

builder.Services.AddSingleton<IChatRepository, ChatRepository>();
builder.Services.AddSingleton<IChatUserRepository, ChatUserRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();


app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => $"{DateTime.UtcNow}: Chat Service is running.");

app.Run();