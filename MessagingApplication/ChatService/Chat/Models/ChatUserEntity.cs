namespace ChatService.Chat.Models
{
    public class ChatUserEntity
    {
        public int ChatId { get; set; }
        public string UserUniqueName { get; set; }
        public DateTimeOffset JoinedAt { get; set; }
        private List<ChatUserPrivilegeModel> privileges = new List<ChatUserPrivilegeModel>();
        public IReadOnlyList<ChatUserPrivilegeModel> Privileges => privileges.AsReadOnly();

        public ChatUserEntity(int chatId, string userUniqueName)
        {
            ChatId = chatId;
            UserUniqueName = userUniqueName;
        }
    }
}
