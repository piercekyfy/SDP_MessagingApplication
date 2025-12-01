using Microsoft.EntityFrameworkCore;
using ChatService.Chat.Models;
using ChatService.Chat.Contexts;
using Shared.Models.Chat;

namespace ChatService.Chat.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatContext context;

        public ChatRepository(ChatContext context)
        {
            this.context = context;
        }

        public async Task<List<ChatEntity>> GetAllChatsAsync()
        {
            return await context.Chats.ToListAsync();
        }

        public async Task<ChatEntity?> GetByIdAsync(int chatId)
        {
            return await context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();
        }

        public async Task CreateChatAsync(ChatEntity chat)
        {
            chat.CreatedAt = DateTimeOffset.UtcNow;
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
        }

        public async Task UpdateChatAsync(int chatId, string chatName)
        {
           ChatEntity? chat = await context.Chats.Where(c => c.Id == chatId).FirstOrDefaultAsync();

            if (chat == null)
                throw new ArgumentException(nameof(chatId));

            chat.Name = chatName;
            context.Chats.Update(chat);
            await context.SaveChangesAsync();
        }

        public async Task<List<ChatUserEntity>> GetAllChatUsersAsync(int chatId)
        {
            return await context.ChatUsers.Include(cu => cu.Privileges).Where(cu => cu.ChatId == chatId).ToListAsync();
        }

        public async Task AddUserAsync(ChatUserEntity user)
        {
            user.JoinedAt = DateTimeOffset.UtcNow;
            await context.ChatUsers.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task SetUserPrivilegeAsync(int chatId, string uniqueName, ChatUserPrivilege privilege, bool value)
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

            var obj = new ChatUserPrivilegeModel(chatId, uniqueName, privilege);
            await context.ChatUserPrivileges.AddAsync(obj);
            await context.SaveChangesAsync();
        }

        public async Task SetUserPrivilegesAsync(int chatId, string uniqueName, ChatUserPrivilege[] privileges, bool value)
        {
            var users = context.ChatUserPrivileges.Where(cup => cup.ChatId == chatId && cup.UserUniqueName == uniqueName);
            if (!value)
            {
                await users.Where(cup => privileges.Contains(cup.Privilege)).ExecuteDeleteAsync();
                await context.SaveChangesAsync();
                return;
            }

            var existingPrivileges = new HashSet<ChatUserPrivilege>(
                await users.Select(c => c.Privilege).ToListAsync()
            );

            await context.ChatUserPrivileges.AddRangeAsync(
                privileges
                    .Where(p => !existingPrivileges.Contains(p))
                    .Select(p => new ChatUserPrivilegeModel(chatId, uniqueName, p)));
            await context.SaveChangesAsync();
        }

        public async Task RemoveUserAsync(int chatId, string uniqueName)
        {
            await context.Database.BeginTransactionAsync();
            try
            {
                await context.ChatUserPrivileges.Where(cup => cup.ChatId == chatId && cup.UserUniqueName == uniqueName).ExecuteDeleteAsync();
                await context.ChatUsers.Where(cu => cu.ChatId == chatId && cu.UserUniqueName == uniqueName).ExecuteDeleteAsync();
                await context.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await context.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
