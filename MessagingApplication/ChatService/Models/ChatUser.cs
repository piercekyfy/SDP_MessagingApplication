namespace ChatService.Models
{
    public class ChatUser
    {
        public int ChatId { get; set; }
        public string UserUniqueName { get; set; }
        public DateTimeOffset JoinedAt { get; set; }
        private List<ChatUserPrivilege> privileges = new List<ChatUserPrivilege>();
        public IReadOnlyList<ChatUserPrivilege> Privileges => privileges.AsReadOnly();

        public ChatUser(int chatId, string userUniqueName)
        {
            ChatId = chatId;
            UserUniqueName = userUniqueName;
        }
    }
}
