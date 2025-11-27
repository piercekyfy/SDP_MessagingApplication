namespace MessageService.Models
{
    public enum ChatUserPrivilege
    {
        IsAdmin = 1,
        CanSend = 2
    }

    public class ChatUser
    {
        public static HashSet<ChatUserPrivilege> AllPermissions = new HashSet<ChatUserPrivilege>() { ChatUserPrivilege.IsAdmin, ChatUserPrivilege.CanSend };

        public string UniqueName { get; set; }
        public HashSet<ChatUserPrivilege> Permissions;

        public ChatUser(string uniqueName, HashSet<ChatUserPrivilege> permissions)
        {
            UniqueName = uniqueName;
            Permissions = permissions;
        }
    }
}
