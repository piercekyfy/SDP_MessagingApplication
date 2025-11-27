using MessageService.Configurations;
using MessageService.Models;
using MessageService.Repositories;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging.Infastructure;
using Shared.Messaging.Models.User;
using System.Text.Json;

namespace MessageService.Messaging.Consumers
{
    public class UsersConsumer : BackgroundService
    {
        private readonly IMessageBrokerChannel channel;
        private readonly IUserRepository repository;

        private readonly UsersQueueConfiguration queue;

        private AsyncEventingBasicConsumer? consumer;

        public UsersConsumer(IMessageBrokerChannel channel, IUserRepository repository, IOptions<UsersQueueConfiguration> configuration) 
        {
            this.channel = channel;
            this.repository = repository;
            this.queue = configuration.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            await channel.DeclareExchange(queue.Exchange, "topic");
            await channel.DeclareQueueAsync(queue.Name, true, false, false, ct);
            await channel.QueueBindAsync(queue.Name, queue.Exchange, queue.RoutingKey, ct);

            consumer = await channel.GetConsumer();
            consumer.ReceivedAsync += MessagedReceivedHandler;
            await channel.BasicConsumeAsync(queue.Name, true, consumer, ct);
        }
        
        protected async Task MessagedReceivedHandler(object sender, BasicDeliverEventArgs args)
        {
            if (args.Body.Span.IsEmpty)
                return;

            if (args.BasicProperties.Type == queue.Events.Created)
            {
#pragma warning disable CS8600 
                UserUpdated ev = JsonSerializer.Deserialize<UserUpdated>(args.Body.Span);
#pragma warning restore CS8600
                if (ev == null)
                    return;

                await repository.CreateUserAsync(new User(ev.UniqueName, ev.DisplayName));
            } else if(args.BasicProperties.Type == queue.Events.Updated)
            {
#pragma warning disable CS8600
                UserUpdated ev = JsonSerializer.Deserialize<UserUpdated>(args.Body.Span);
#pragma warning restore CS8600
                if (ev == null)
                    return;

                await repository.UpdateUserAsync(ev.UniqueName, ev.DisplayName);
            }
            else
            {
                throw new Exception("Unexpected event type in queue.");
            }
        }
    }
}
