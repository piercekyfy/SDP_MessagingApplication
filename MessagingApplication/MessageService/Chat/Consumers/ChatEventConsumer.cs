using ChatService.Chat.Events;
using MessageService.Chat.Observers;
using MessageService.Configurations;
using MessageService.User.Observers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using Shared.Messaging.Models.User;
using Shared.Middleware.Messaging;
using System.Text.Json;

namespace MessageService.Chat.Consumers
{
    public class ChatEventConsumer : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ChatQueueConfiguration queue;

        private AsyncEventingBasicConsumer? consumer;

        public ChatEventConsumer(IServiceProvider serviceProvider, IOptions<ChatQueueConfiguration> configuration)
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
            var handler = scope.ServiceProvider.GetRequiredService<ChatEventHandler>();

            if (args.BasicProperties.Type == queue.Events.Created && Shared.Utility.TryDeserialize<ChatCreated>(args, out var created))
                await handler.HandleChatCreatedAsync(created);
            else if (args.BasicProperties.Type == queue.Events.UserJoined && Shared.Utility.TryDeserialize<UserJoinedChat>(args, out var joined))
                await handler.HandleUserJoinedAsync(joined);
            else
                throw new Exception("Unexpected event type in queue.");
        }
    }
}
