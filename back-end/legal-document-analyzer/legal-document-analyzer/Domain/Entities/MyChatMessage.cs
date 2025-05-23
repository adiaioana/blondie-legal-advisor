using System.Text.Json.Serialization;

namespace legal_document_analyzer.Domain.Entities
{
    public class MyChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatSessionId { get; set; }
        [JsonIgnore]
        public ChatSession ChatSession { get; set; } = default!;

        public string Sender { get; set; } = "user"; // "user" or "assistant"
        public string Content { get; set; } = string.Empty;
        public string InputMode { get; set; } = "text"; // "text" or "speech"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
