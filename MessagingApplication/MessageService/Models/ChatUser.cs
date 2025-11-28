namespace MessageService.Models
{
    public enum ChatUserPrivilege
    {
        IsAdmin = 1,
        CanSend = 2,
        CanForward = 3
    }

    public class ChatUser
    {
        public static HashSet<ChatUserPrivilege> AllPermissions = new HashSet<ChatUserPrivilege>() { ChatUserPrivilege.IsAdmin, ChatUserPrivilege.CanSend, ChatUserPrivilege.CanForward };

        public string UniqueName { get; set; }
        public HashSet<ChatUserPrivilege> Permissions;

        public ChatUser(string uniqueName, HashSet<ChatUserPrivilege> permissions)
        {
            UniqueName = uniqueName;
            Permissions = permissions;
        }
    }
}
