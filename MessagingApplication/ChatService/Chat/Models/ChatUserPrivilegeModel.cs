using Shared.Models.Chat;

namespace ChatService.Chat.Models
{
    public class ChatUserPrivilegeModel
    {
        public static ChatUserPrivilege[] All = new[] { ChatUserPrivilege.IsAdmin, ChatUserPrivilege.CanSend, ChatUserPrivilege.CanForward };

        public int ChatId { get; set; }
        public string UserUniqueName { get; set; }
        public ChatUserPrivilege Privilege { get; set; }

        public ChatUserPrivilegeModel(int chatId, string userUniqueName, ChatUserPrivilege privilege)
        {
            ChatId = chatId;
            UserUniqueName = userUniqueName;
            Privilege = privilege;
        }
    }
}
