namespace ChatService.Chat.DTOs
{
    public class GetChatUserResponse
    {
        public string UniqueName { get; set; }
        public int[] Privileges { get; set; }
        public DateTimeOffset JoinedAt { get; set; }

        public GetChatUserResponse(string uniqueName, int[] privileges, DateTimeOffset joinedAt)
        {
            UniqueName = uniqueName;
            Privileges = privileges;
            JoinedAt = joinedAt;
        }
    }
}
