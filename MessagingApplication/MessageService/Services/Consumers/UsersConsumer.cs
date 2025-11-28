using MessageService.Configurations;
using MessageService.Models;
using MessageService.Repositories;
using MessageService.Services.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using Shared.Messaging.Infastructure;
using Shared.Messaging.Models.User;
using System.Text.Json;

namespace MessageService.Services.Consumers
{
    public class UsersConsumer : BackgroundService
    {
        private readonly IMessageBrokerChannel channel;
        private readonly IServiceProvider serviceProvider;

        private readonly UsersQueueConfiguration queue;

        private AsyncEventingBasicConsumer? consumer;

        public UsersConsumer(IMessageBrokerChannel channel, IServiceProvider serviceProvider, IOptions<UsersQueueConfiguration> configuration) 
        {
            this.channel = channel;
            this.serviceProvider = serviceProvider;
            queue = configuration.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            await channel.DeclareExchange(queue.Exchange, "topic");
            await channel.DeclareQueueAsync(queue.Name, true, false, false, ct);
            await channel.QueueBindAsync(queue.Name, queue.Exchange, queue.RoutingKey, ct);

            consumer = await channel.GetConsumer();
            consumer.ReceivedAsync -= MessagedReceivedHandler;
            consumer.ReceivedAsync += MessagedReceivedHandler;
            await channel.BasicConsumeAsync(queue.Name, true, consumer, ct);
        }
        
        protected async Task MessagedReceivedHandler(object sender, BasicDeliverEventArgs args)
        {
            if (args.Body.Span.IsEmpty)
                return;

            using var scope = serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IUserEventHandler>();

            if (args.BasicProperties.Type == queue.Events.Created && Shared.Utility.TryDeserialize<UserUpdated>(args, out var created))
                await handler.HandleUserCreatedAsync(created);
            else if (args.BasicProperties.Type == queue.Events.Updated && Shared.Utility.TryDeserialize<UserUpdated>(args, out var updated))
                await handler.HandleUserCreatedAsync(updated);
            else if (args.BasicProperties.Type == queue.Events.Deleted && Shared.Utility.TryDeserialize<UserDeleted>(args, out var deleted))
                await handler.HandleUserDeletedAsync(deleted);
            else
                throw new Exception("Unexpected event type in queue.");
        }
    }
}
