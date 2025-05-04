using legal_document_analyzer.Domain.Entities;

namespace legal_document_analyzer.Domain.Repositories
{
    public interface IDocumentSummaryRepository
    {
        Task<DocumentSummary> GetSummaryByIdAsync(Guid summaryId);
        Task<DocumentSummary> GetSummaryByDocumentIdAsync(Guid documentId);
        Task AddSummaryAsync(DocumentSummary summary);
        Task UpdateSummaryAsync(DocumentSummary summary);
        Task DeleteSummaryAsync(Guid summaryId);
        Task AddSummaryAsync(Task<DocumentSummary> summary);
    }
}
