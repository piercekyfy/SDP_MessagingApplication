using Shared.Models.Chat;

namespace MessageService.Chat.Models
{
    public class ChatUserModel
    {
        public string UniqueName { get; set; }
        public List<ChatUserPrivilege> Privileges { get; set; } = new List<ChatUserPrivilege>();

        public ChatUserModel(string uniqueName, List<ChatUserPrivilege> privileges)
        {
            UniqueName = uniqueName;
            Privileges = privileges.Distinct().ToList();
        }
    }
}
