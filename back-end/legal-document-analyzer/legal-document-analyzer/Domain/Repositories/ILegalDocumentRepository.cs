using legal_document_analyzer.Domain.Entities;

namespace legal_document_analyzer.Domain.Repositories
{
    public interface ILegalDocumentRepository
    {
        Task<LegalDocument> GetDocumentByIdAsync(Guid documentId);
        Task<IEnumerable<LegalDocument>> GetAllDocumentsAsync();
        Task<(IEnumerable<Clause> Clauses, DocumentSummary Summary)> AddDocumentAsync(LegalDocument document);
        Task UpdateDocumentAsync(LegalDocument document);
        Task DeleteDocumentAsync(Guid documentId);
        Task<IEnumerable<LegalDocument>> GetDocumentsByUserIdAsync(Guid userId);
    }
}
