using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace legal_document_analyzer.Infrastructure.Persistence
{

    public class DocumentSummaryRepository : IDocumentSummaryRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentSummaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentSummary> GetSummaryByIdAsync(Guid summaryId)
        {
            return await _context.DocumentSummaries
                .FirstOrDefaultAsync(s => s.DocumentSummaryId == summaryId);
        }
        public async Task AddSummaryAsync(DocumentSummary summary)
        {
            await _context.DocumentSummaries.AddAsync(summary);
            await _context.SaveChangesAsync();
        }
        public async Task<DocumentSummary> GetSummaryByDocumentIdAsync(Guid documentId)
        {
            return await _context.DocumentSummaries
                .FirstOrDefaultAsync(s => s.DocumentId == documentId);
        }

        public async Task AddSummaryAsync(Task<DocumentSummary> summary)
        {

            if (summary == null)
                throw new ArgumentNullException(nameof(summary));

            var summaryAw = await summary.ConfigureAwait(false);
            await _context.DocumentSummaries.AddAsync(summaryAw);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSummaryAsync(DocumentSummary summary)
        {
            _context.DocumentSummaries.Update(summary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSummaryAsync(Guid summaryId)
        {
            var summary = await _context.DocumentSummaries.FindAsync(summaryId);
            if (summary != null)
            {
                _context.DocumentSummaries.Remove(summary);
                await _context.SaveChangesAsync();
            }
        }

    }

}