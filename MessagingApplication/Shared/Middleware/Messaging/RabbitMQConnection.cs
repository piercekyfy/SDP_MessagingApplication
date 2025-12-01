using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Configurations;
using Shared.Messaging.Models;

namespace Shared.Middleware.Messaging
{
    public class RabbitMQConnection : IMessageBrokerConnection
    {
        private readonly ConnectionFactory connectionFactory;

        private IConnection? connection;

        public RabbitMQConnection(IOptions<RabbitMQConfiguration> configuration)
        {
            connectionFactory = new ConnectionFactory() { Uri = new Uri(configuration.Value.ConnectionString ?? "") };
        }

        public async Task<IChannel> GetChannel()
        {
            if(connection == null)
                connection = await connectionFactory.CreateConnectionAsync();
            return await connection.CreateChannelAsync();
        }
    }
}
