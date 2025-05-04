using legal_document_analyzer.Domain.Entities;
using System.Collections.Generic;

namespace legal_document_analyzer.Infrastructure.Services
{
    public interface ILegalDocumentParser
    {
        IEnumerable<Clause> ParseClauses(string content, Guid documentId);
        Task<DocumentSummary> GenerateSummary(string content, Guid documentId);
    }
}
