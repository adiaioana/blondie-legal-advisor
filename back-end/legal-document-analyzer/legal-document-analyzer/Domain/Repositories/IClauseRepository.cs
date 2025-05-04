using legal_document_analyzer.Domain.Entities;

namespace legal_document_analyzer.Domain.Repositories
{
    public interface IClauseRepository
    {
        Task<Clause> GetClauseByIdAsync(Guid clauseId);
        Task<IEnumerable<Clause>> GetClausesByDocumentIdAsync(Guid documentId);
        Task AddClauseAsync(Clause clause);
        Task UpdateClauseAsync(Clause clause);
        Task DeleteClauseAsync(Guid clauseId);
    }
}
