namespace MessageService.Configurations
{
    public class UsersQueueEvents
    {
        public string Created { get; set; } = default!;
        public string Deleted { get; set; } = default!;
    }

    public class UsersQueueConfiguration
    {
        public string Name { get; set; } = default!;
        public string Exchange { get; set; } = default!;
        public string RoutingKey { get; set; } = default!;
        public UsersQueueEvents Events { get; set; } = default!;
    }
}
