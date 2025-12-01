using ChatService.Chat.Models;
using Shared.Models.Chat;

namespace ChatService.Chat.Repositories
{
    public interface IChatRepository
    {
        Task<List<ChatEntity>> GetAllChatsAsync();
        Task<ChatEntity?> GetByIdAsync(int chatId);
        Task CreateChatAsync(ChatEntity chat);
        Task UpdateChatAsync(int chatId, string chatName);

        Task<List<ChatUserEntity>> GetAllChatUsersAsync(int chatId);
        Task AddUserAsync(ChatUserEntity user);
        Task SetUserPrivilegeAsync(int chatId, string uniqueName, ChatUserPrivilege privilege, bool value);
        Task SetUserPrivilegesAsync(int chatId, string uniqueName, ChatUserPrivilege[] privileges, bool value);
        Task RemoveUserAsync(int chatId, string uniqueName);
    }
}
