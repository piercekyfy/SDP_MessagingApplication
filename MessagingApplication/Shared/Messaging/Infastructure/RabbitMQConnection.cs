using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Shared.Messaging.Models;

namespace Shared.Messaging.Infastructure
{
    public class RabbitMQConnection : IMessageBrokerConnection
    {
        private readonly ConnectionFactory connectionFactory;

        private IConnection? connection;

        public RabbitMQConnection(IConfiguration configuration)
        {
            connectionFactory = new ConnectionFactory() { Uri = new Uri(configuration["RabbitMQ:ConnectionString"] ?? "") };
        }

        public async Task<IChannel> GetChannel()
        {
            if(connection == null)
                connection = await connectionFactory.CreateConnectionAsync();
            return await connection.CreateChannelAsync();
        }
    }
}
