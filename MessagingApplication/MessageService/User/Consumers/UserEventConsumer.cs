using MessageService.Configurations;
using MessageService.User.Observers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using Shared.Messaging.Models.User;
using Shared.Middleware.Messaging;

namespace MessageService.User.Consumers
{
    public class UserEventConsumer : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        private readonly UserQueueConfiguration queue;

        private AsyncEventingBasicConsumer? consumer;

        public UserEventConsumer(IServiceProvider serviceProvider, IOptions<UserQueueConfiguration> configuration) 
        {
            this.serviceProvider = serviceProvider;
            queue = configuration.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var scope = serviceProvider.CreateScope();
            using var channel = serviceProvider.GetRequiredService<IMessageBrokerChannel>();

            await channel.DeclareExchange(queue.Exchange, "topic");
            await channel.DeclareQueueAsync(queue.Name, true, false, false, ct);
            await channel.QueueBindAsync(queue.Name, queue.Exchange, queue.RoutingKey, ct);

            consumer = await channel.GetConsumer();
            consumer.ReceivedAsync -= MessagedReceivedHandler;
            consumer.ReceivedAsync += MessagedReceivedHandler;

            await channel.BasicConsumeAsync(queue.Name, true, consumer, ct);

            await Task.Delay(Timeout.Infinite, ct);
        }
        
        protected async Task MessagedReceivedHandler(object sender, BasicDeliverEventArgs args)
        {
            if (args.Body.Span.IsEmpty)
                return;

            using var scope = serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<UserEventHandler>();

            if (args.BasicProperties.Type == queue.Events.Created && Shared.Utility.TryDeserialize<UserUpdated>(args, out var created))
                await handler.HandleUserCreatedAsync(created);
            else if (args.BasicProperties.Type == queue.Events.Deleted && Shared.Utility.TryDeserialize<UserDeleted>(args, out var deleted))
                await handler.HandleUserDeletedAsync(deleted);
            else
                throw new Exception("Unexpected event type in queue.");
        }
    }
}
