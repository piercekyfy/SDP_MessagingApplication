namespace MessageService.Configurations
{
    public class UserQueueEvents
    {
        public string Created { get; set; } = default!;
        public string Updated { get; set; } = default!;
        public string Deleted { get; set; } = default!;
    }

    public class UserQueueConfiguration
    {
        public string Name { get; set; } = default!;
        public string Exchange { get; set; } = default!;
        public string RoutingKey { get; set; } = default!;
        public UserQueueEvents Events { get; set; } = default!;
    }
}
