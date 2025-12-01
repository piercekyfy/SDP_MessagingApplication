namespace ChatService.Chat.DTOs
{
    public class AddUserRequest
    {
        public string? UniqueName { get; set; }
        public int[]? Privileges { get; set; }

        public AddUserRequest() {}

        public AddUserRequest(string uniqueName, int[] privileges)
        {
            UniqueName = uniqueName;
            Privileges = privileges;
        }
    }
}
