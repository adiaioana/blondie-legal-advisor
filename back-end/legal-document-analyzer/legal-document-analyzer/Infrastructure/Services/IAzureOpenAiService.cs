using legal_document_analyzer.Domain.Entities;

namespace legal_document_analyzer.Infrastructure.Services
{
    public interface IAzureOpenAiService
    {
        Task<string> GetCompletionAsync(List<MyChatMessage> chatHistory);
    }

}
