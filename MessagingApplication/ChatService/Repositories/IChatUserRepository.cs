using ChatService.Models;

namespace ChatService.Repositories
{
    public interface IChatUserRepository
    {
        Task CreateAsync(ChatUser user);
        Task SetPrivilege(int chatId, string uniqueName, Shared.Models.ChatUserPrivilege privilege, bool value);
        Task DeleteAsync(int chatId, string uniqueName);
    }
}
