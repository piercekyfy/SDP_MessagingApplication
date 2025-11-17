using System.Text.Json;

using UserService.Models;
using Shared.Messaging.Infastructure;
using Shared.Messaging.Models.User;
using System.Text;


namespace UserService.Services
{
    public class UsersExchangeService : IUsersExchangeService
    {
        private readonly IMessageBrokerChannel channel;
        private readonly string exchange;
        private readonly string createdType;
        private readonly string createdRoute;

        public UsersExchangeService(IMessageBrokerChannel connection, IConfiguration configuration) 
        { 
            this.channel = connection;

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
            createdType = GetConfigStringOrThrow(configuration, "Exchanges:Users:Events:Created:Name");
            createdRoute = GetConfigStringOrThrow(configuration, "Exchanges:Users:Events:Created:Route");
        }

        public async Task PublishUserCreated(User user)
        {
            await channel.DeclareExchange(exchange, "topic");

            await channel.BasicPublish(exchange, createdRoute, new UserCreated(user.UniqueName), createdType);
        }
    }
}
