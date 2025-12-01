using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Middleware.Messaging
{
    public enum DeliveryMode
    {
        Transient,
        Persistent
    }

    public interface IMessageBrokerChannel : IDisposable
    {
        public Task DeclareExchange(string exchange, string type);
        public Task DeclareQueueAsync(string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, CancellationToken ct = default);
        public Task QueueBindAsync(string queue, string exchange, string routingKey, CancellationToken ct = default);
        public Task BasicConsumeAsync(string queue, bool autoAck, IAsyncBasicConsumer consumer, CancellationToken ct = default);
        public Task BasicPublish(string exchange, string routingKey, MessagingModel message, string type, bool mandatory = false, DeliveryMode deliveryMode = DeliveryMode.Persistent, CancellationToken ct = default);
        public Task<AsyncEventingBasicConsumer> GetConsumer(); // A wrapper for RabbitMQ's IAsyncBasicConsumer should be created
    }
}
