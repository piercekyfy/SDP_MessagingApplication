using Shared.Configurations;

namespace UserService.Configurations
{
    public class UserExchangeEvents
    {
        public ExchangeEvent Created { get; set; } = default!;
        public ExchangeEvent Updated { get; set; } = default!;
        public ExchangeEvent Deleted { get; set; } = default!;
    }

    public class UsersExchangeConfiguration
    {
        public string Name { get; set; } = default!;
        public UserExchangeEvents Events { get; set; } = default!;
    }
}
