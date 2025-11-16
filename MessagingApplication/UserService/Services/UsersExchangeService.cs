using System.Text.Json;

using UserService.Models;
using Shared.Messaging.Infastructure;
using Shared.Messaging.Models.User;
using System.Text;


namespace UserService.Services
{
    public class UsersExchangeService : IUsersExchangeService
    {
        private readonly IMessageBrokerConnection connection;
        private readonly string exchange;
        private readonly string createdRoute;

        public UsersExchangeService(IMessageBrokerConnection connection, IConfiguration configuration) 
        { 
            this.connection = connection;

            string GetConfigStringOrThrow(IConfiguration configuration, string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(configuration[key]))
                        throw new ArgumentException($"Exchange Configuration {key} must be specified.");
                } catch (IndexOutOfRangeException)
                {
                    throw new ArgumentException($"Exchange Configuration {key} must be specified.");
                }
                return configuration[key] ?? "";
            }

            exchange = GetConfigStringOrThrow(configuration, "Exchanges:Users:Name");
            createdRoute = GetConfigStringOrThrow(configuration, "Exchanges:Users:Routes:Created");
        }

        public async Task PublishUserCreated(User user)
        {
            await connection.DeclareExchange(exchange, "topic");

            await connection.BasicPublish(exchange, createdRoute, new UserCreated(user.UniqueName));
        }
    }
}
