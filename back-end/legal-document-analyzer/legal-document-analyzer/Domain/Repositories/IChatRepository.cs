using legal_document_analyzer.Domain.Entities;

namespace legal_document_analyzer.Domain.Repositories
{
    public interface IChatRepository
    {
        Task<ChatSession?> GetActiveSessionAsync(string userId);
        Task<ChatSession> CreateSessionAsync(string userId);
        Task EndSessionAsync(Guid sessionId);
        Task AddMessageAsync(Guid sessionId, string sender, string content, string inputMode);
        Task<List<MyChatMessage>> GetMessagesAsync(Guid sessionId);
    }

}
