namespace MessageService.Configurations
{
    public class ChatQueueEvents
    {
        public string Created { get; set; } = default!;
        public string UserJoined { get; set; } = default!;

    }

    public class ChatQueueConfiguration
    {
        public string Name { get; set; } = default!;
        public string Exchange { get; set; } = default!;
        public string RoutingKey { get; set; } = default!;
        public ChatQueueEvents Events { get; set; } = default!;
    }
}
