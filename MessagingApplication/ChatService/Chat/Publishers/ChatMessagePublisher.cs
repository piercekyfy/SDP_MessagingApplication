using ChatService.Chat.Events;
using ChatService.Configurations;
using Microsoft.Extensions.Options;
using Shared.Middleware.Messaging;

namespace ChatService.Chat.Publishers
{
    public class ChatMessagePublisher
    {
        private readonly IMessageBrokerChannel channel;
        private readonly ChatExchangeConfiguration exchange;

        public ChatMessagePublisher(IMessageBrokerChannel connection, IOptions<ChatExchangeConfiguration> configuration)
        {
            this.channel = connection;
            this.exchange = configuration.Value;
        }

        public async Task PublishCreatedAsync(ChatCreated ev)
        {
            await channel.DeclareExchange(exchange.Name, "topic");
            await channel.BasicPublish(exchange.Name, exchange.Events.Created.Route, ev, exchange.Events.Created.Name);
        }

        public async Task PublishJoinedAsync(UserJoinedChat ev)
        {
            await channel.DeclareExchange(exchange.Name, "topic");
            await channel.BasicPublish(exchange.Name, exchange.Events.UserJoinedChat.Route, ev, exchange.Events.UserJoinedChat.Name);
        }
    }
}
