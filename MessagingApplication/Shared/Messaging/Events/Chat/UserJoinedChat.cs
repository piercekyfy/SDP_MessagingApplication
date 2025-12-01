using Shared.Messaging.Models;

namespace ChatService.Chat.Events
{
    public class UserJoinedChat : MessagingModel
    {
        public int ChatId { get; set; }
        public string UniqueName { get; set; }
        public int[] Privileges { get; set; }
        public DateTimeOffset JoinedAt { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UserJoinedChat() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public UserJoinedChat(int chatId, string uniqueName, int[] permissions, DateTimeOffset joinedAt)
        {
            ChatId = chatId;
            UniqueName = uniqueName;
            Privileges = permissions;
            JoinedAt = joinedAt;
        }
    }
}
