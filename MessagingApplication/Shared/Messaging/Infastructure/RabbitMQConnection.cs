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

            await channel.ExchangeDeclareAsync(exchange, type, durable: true, autoDelete: false);
        }

        public async Task BasicPublish(string exchange, string routingKey, MessageModel message, DeliveryMode deliveryMode = DeliveryMode.Persistent, CancellationToken ct = default)
        {
            if (channel == null)
                throw new InvalidOperationException("Cannot publish, RabbitMQ is not connected.");

            var props = new BasicProperties()
            {
                ContentType = "application/json",
                DeliveryMode = deliveryMode == DeliveryMode.Transient ? DeliveryModes.Transient : DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(exchange, routingKey, message.AsJsonBytes(), ct);
        }
    }
}
