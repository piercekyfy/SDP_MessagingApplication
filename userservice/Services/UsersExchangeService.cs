using System.Text.Json;

using UserService.Models;
using Shared.Messaging.Infastructure;
using UserService.Models.Messaging;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace UserService.Services
{
    public class UsersExchangeService // TODO: Create interface
    {
        private readonly IMessageBrokerConnection connection;
        private readonly string exchange;
        private readonly string createdRoute;

        public UsersExchangeService(IMessageBrokerConnection connection, IConfiguration configuration) 
        { 
            this.connection = connection;

            string GetConfigStringOrThrow(IConfiguration configuration, string key)
            {
                if (string.IsNullOrEmpty(configuration[key]))
                    throw new ArgumentException($"Exchange Configuration {key} must be specified.");
                return configuration["Exchange:Users:Name"] ?? "";
            }

            exchange = GetConfigStringOrThrow(configuration, "Users:Name");
            createdRoute = GetConfigStringOrThrow(configuration, "Users:Routes:Created");
        }

        public async Task PublishUserCreated(User user)
        {
            await connection.DeclareExchange(exchange, "topic");

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(
                    new UserCreated(user.UniqueName)
                    )
                );

            await connection.BasicPublish(exchange, createdRoute, body);
        }
    }
}
