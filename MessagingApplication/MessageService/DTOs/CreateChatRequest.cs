namespace MessageService.DTOs
{
    public class CreateChatRequest
    {
        public string Name { get; set; }
        public string OwnerUniqueName { get; set; }

        public CreateChatRequest(string name, string ownerUniqueName)
        {
            Name = name;
            OwnerUniqueName = ownerUniqueName;
        }
    }
}
