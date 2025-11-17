using MessageService.Repositories;
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

        private readonly string usersQueue;
        private readonly string usersExchange;
        private readonly string usersRoutingKey;

        private readonly string userCreatedEvent;

        private AsyncEventingBasicConsumer? consumer;

        public UsersConsumer(IMessageBrokerChannel channel, IUserRepository repository, IConfiguration configuration) 
        {
            this.channel = channel;
            this.repository = repository;

            string GetConfigStringOrThrow(IConfiguration configuration, string key)
            {
                try
                {
                    if (string.IsNullOrEmpty(configuration[key]))
                        throw new ArgumentException($"Configuration {key} must be specified.");
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ArgumentException($"Configuration {key} must be specified.");
                }
                return configuration[key] ?? "";
            }

            usersQueue = GetConfigStringOrThrow(configuration, "Queues:Users:Name");
            usersExchange = GetConfigStringOrThrow(configuration, "Queues:Users:Exchange");
            usersRoutingKey = GetConfigStringOrThrow(configuration, "Queues:Users:RoutingKey");

            userCreatedEvent = GetConfigStringOrThrow(configuration, "Queues:Users:Events:Created");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await channel.DeclareQueueAsync(usersQueue, true, false, false, stoppingToken);
            await channel.QueueBindAsync(usersQueue, usersExchange, usersRoutingKey, stoppingToken);

            consumer = await channel.GetConsumer();
            consumer.ReceivedAsync += MessagedReceivedHandler;
            await channel.BasicConsumeAsync(usersQueue, true, consumer, stoppingToken);
        }
        
        protected async Task MessagedReceivedHandler(object sender, BasicDeliverEventArgs args)
        {
            if (args.Body.Span.IsEmpty)
                return;

            if (args.BasicProperties.Type == userCreatedEvent)
            {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                UserCreated ev = JsonSerializer.Deserialize<UserCreated>(args.Body.Span);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (ev == null)
                    return;

                await repository.CreateUserAsync(new Models.User(ev.UniqueName));
            } else
            {
                throw new Exception("Unexpected event type in queue.");
            }
        }
    }
}
