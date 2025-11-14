using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Shared.Messaging.Infastructure
{
    public class RabbitMQConnection : IMessageBrokerConnection
    {
        private readonly ConnectionFactory connectionFactory;

        private IConnection? connection;
        private IChannel? channel;

        public RabbitMQConnection(IConfiguration configuration)
        {
            connectionFactory = new ConnectionFactory() { Uri = new Uri(configuration["RabbitMQ:ConnectionString"] ?? "") };
        }

        public async Task Connect()
        {
            connection = await connectionFactory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            
        }

        public async Task DeclareExchange(string exchange, string type)
        {
            if (channel == null)
                throw new InvalidOperationException("Cannot declare exchange, RabbitMQ is not connected.");

            await channel.ExchangeDeclareAsync(exchange, type);
        }

        public async Task BasicPublish(string exchange, string routingKey, ReadOnlyMemory<byte> body, CancellationToken ct = default)
        {
            if (channel == null)
                throw new InvalidOperationException("Cannot publish, RabbitMQ is not connected.");

            await channel.BasicPublishAsync(exchange, routingKey, body, ct);
        }
    }
}
