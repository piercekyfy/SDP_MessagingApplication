using Shared.Configurations;

namespace ChatService.Configurations
{
    public class ChatExchangeEvents
    {
        public ExchangeEvent Created { get; set; } = default!;
        public ExchangeEvent Updated { get; set; } = default!;
        public ExchangeEvent UserJoinedChat { get; set; } = default!;
        public ExchangeEvent ChatUserPrivilegeUpdated { get; set; } = default!;
        public ExchangeEvent UserLeftChat { get; set; } = default!;
    }

    public class ChatExchangeConfiguration
    {
        public string Name { get; set; } = default!;
        public ChatExchangeEvents Events { get; set; } = default!;
    }
}
