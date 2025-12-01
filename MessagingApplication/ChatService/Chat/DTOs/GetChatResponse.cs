namespace ChatService.Chat.DTOs
{
    public class GetChatResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public GetChatResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
