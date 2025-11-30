
namespace ChatService.Models
{
    public class ChatUserPrivilege
    {
        public int ChatId { get; set; }
        public string UserUniqueName { get; set; }
        public Shared.Models.ChatUserPrivilege Privilege { get; set; }

        public ChatUserPrivilege(int chatId, string userUniqueName, Shared.Models.ChatUserPrivilege privilege)
        {
            ChatId = chatId;
            UserUniqueName = userUniqueName;
            Privilege = privilege;
        }
    }
}
