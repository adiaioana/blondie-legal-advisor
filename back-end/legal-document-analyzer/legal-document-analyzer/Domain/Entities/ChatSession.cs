namespace legal_document_analyzer.Domain.Entities
{
    public class ChatSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public List<MyChatMessage> Messages { get; set; } = new();
    }

}
