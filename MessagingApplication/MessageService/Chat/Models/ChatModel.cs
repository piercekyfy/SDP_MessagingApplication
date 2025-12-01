namespace MessageService.Chat.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public List<ChatUserModel> Users { get; set; } = new List<ChatUserModel>();

        public ChatModel(int id, string name)
        {
            Id = id;
        }
    }
}
