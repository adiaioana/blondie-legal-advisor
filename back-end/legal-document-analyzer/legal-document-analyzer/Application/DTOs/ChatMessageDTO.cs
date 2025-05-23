namespace legal_document_analyzer.Application.DTOs
{
    public class ChatMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public string InputMode { get; set; } = "text"; // "text" or "speech"
    }

}
