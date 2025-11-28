using System.Text.Json;

using UserService.Models;
using Shared.Messaging.Infastructure;
using Shared.Messaging.Models.User;
using System.Text;
using UserService.Configurations;
using Microsoft.Extensions.Options;


namespace UserService.Services
{
    public class UserExchangeService : IUserExchangeService
    {
        private readonly IMessageBrokerChannel channel;
        private readonly UsersExchangeConfiguration exchange;

        public UserExchangeService(IMessageBrokerChannel connection, IOptions<UsersExchangeConfiguration> configuration) 
        { 
            this.channel = connection;
            this.exchange = configuration.Value;

        }

        public async Task PublishCreatedAsync(User user)
        {
            await channel.DeclareExchange(exchange.Name, "topic");

            await channel.BasicPublish(exchange.Name, exchange.Events.Created.Route, new UserUpdated(user.UniqueName, user.DisplayName), exchange.Events.Created.Name);
        }

        public async Task PublishUpdatedAsync(User user)
        {
            await channel.DeclareExchange(exchange.Name, "topic");

            await channel.BasicPublish(exchange.Name, exchange.Events.Updated.Route, new UserUpdated(user.UniqueName, user.DisplayName), exchange.Events.Updated.Name);
        }

        public async Task PublishDeletedAsync(User user)
        {
            await channel.DeclareExchange(exchange.Name, "topic");

            await channel.BasicPublish(exchange.Name, exchange.Events.Deleted.Route, new UserDeleted(user.UniqueName, user.DeletedAt), exchange.Events.Deleted.Name);
        }
    }
}
