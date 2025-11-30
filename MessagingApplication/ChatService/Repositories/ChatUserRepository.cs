using Microsoft.EntityFrameworkCore;
using ChatService.Contexts;
using ChatService.Models;

namespace ChatService.Repositories
{
    public class ChatUserRepository : IChatUserRepository
    {
        private readonly ChatsContext context;

        public ChatUserRepository(ChatsContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(ChatUser user)
        {
            user.JoinedAt = DateTimeOffset.UtcNow;
            await context.ChatUsers.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task SetPrivilege(int chatId, string uniqueName, Shared.Models.ChatUserPrivilege privilege, bool value)
        {
            var query = context.ChatUserPrivileges.Where(cup => cup.ChatId == chatId && cup.UserUniqueName == uniqueName && cup.Privilege == privilege);
            if (!value)
            {
                await query.ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                return;
            }

            if (await query.FirstOrDefaultAsync() != null)
                return;

            var obj = new ChatUserPrivilege(chatId, uniqueName, privilege);
            await context.ChatUserPrivileges.AddAsync(obj);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int chatId, string uniqueName)
        {
            await context.Database.BeginTransactionAsync();
            try
            {
                await context.ChatUserPrivileges.Where(cup => cup.ChatId == chatId && cup.UserUniqueName == uniqueName).ExecuteDeleteAsync();
                await context.ChatUsers.Where(cu => cu.ChatId == chatId && cu.UserUniqueName == uniqueName).ExecuteDeleteAsync();
                await context.Database.CommitTransactionAsync();
            } catch (Exception)
            {
                await context.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
