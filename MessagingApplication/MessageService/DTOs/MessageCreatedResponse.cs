namespace MessageService.DTOs
{
    public class MessageCreatedResponse
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public MessageCreatedResponse(string id, DateTimeOffset timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}
