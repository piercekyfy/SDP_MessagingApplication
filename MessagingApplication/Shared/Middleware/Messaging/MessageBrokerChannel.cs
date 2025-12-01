using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Shared.Middleware.Messaging
{
    public class MessageBrokerChannel : IMessageBrokerChannel
    {
        private readonly IMessageBrokerConnection connection;
        private IChannel? channel;
        public MessageBrokerChannel(IMessageBrokerConnection connection)
        {
            this.connection = connection;
        }

        public async Task DeclareExchange(string exchange, string type)
        {
            var ch = await GetChannel();

            await ch.ExchangeDeclareAsync(exchange, type, durable: true, autoDelete: false);
        }

        public async Task DeclareQueueAsync(string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, CancellationToken ct = default)
        {
            var ch = await GetChannel();

            await ch.QueueDeclareAsync(queue, durable: durable, exclusive: exclusive, autoDelete: autoDelete, cancellationToken: ct);
        }

        public async Task QueueBindAsync(string queue, string exchange, string routingKey, CancellationToken ct = default)
        {
            var ch = await GetChannel();

            await ch.QueueBindAsync(queue, exchange, routingKey, cancellationToken: ct);
        }

        public async Task BasicConsumeAsync(string queue, bool autoAck, IAsyncBasicConsumer consumer, CancellationToken ct = default)
        {
            var ch = await GetChannel();
            
            await ch.BasicConsumeAsync(queue, autoAck, consumer, ct);
        }

        public async Task BasicPublish(string exchange, string routingKey, MessagingModel message, string type, bool mandatory = false, DeliveryMode deliveryMode = DeliveryMode.Persistent, CancellationToken ct = default)
        {
            var ch = await GetChannel();

            var props = new BasicProperties()
            {
                ContentType = "application/json",
                Type = type,
                DeliveryMode = deliveryMode == DeliveryMode.Transient ? DeliveryModes.Transient : DeliveryModes.Persistent,
            };

            await ch.BasicPublishAsync(exchange, routingKey, mandatory, props, message.AsJsonBytes(), ct);
        }

        public async Task<AsyncEventingBasicConsumer> GetConsumer() 
        {
            return new AsyncEventingBasicConsumer(await GetChannel());
        }

        protected async Task<IChannel> GetChannel()
        {
            if (channel != null)
                return await Task.FromResult(channel);
            else
            {
                channel = await connection.GetChannel();
                return channel;
            }
        }

        public void Dispose()
        {
            channel?.Dispose();
        }
    }
}
