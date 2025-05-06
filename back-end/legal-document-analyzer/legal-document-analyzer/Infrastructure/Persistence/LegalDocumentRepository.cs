using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using legal_document_analyzer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UglyToad.PdfPig;

namespace legal_document_analyzer.Infrastructure.Persistence
{
    public class LegalDocumentRepository : ILegalDocumentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILegalDocumentParser _parser;

        public LegalDocumentRepository(ApplicationDbContext context, ILegalDocumentParser parser)
        {
            _context = context;
            _parser = parser;
        }

        public async Task<LegalDocument> GetDocumentByIdAsync(Guid documentId)
        {
            return await _context.Set<LegalDocument>()
                .Include(d => d.Clauses)
                .Include(d => d.Summary)
                .FirstOrDefaultAsync(d => d.LegalDocumentId == documentId);
        }

        public async Task<IEnumerable<LegalDocument>> GetAllDocumentsAsync()
        {
            return await _context.Set<LegalDocument>()
                .Include(d => d.Clauses)
                .Include(d => d.Summary)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Clause> Clauses, DocumentSummary Summary)> AddDocumentAsync(LegalDocument document)
        {
            Console.WriteLine(document.Content.Length);
            var slicedContent = document.Content;
            var parsedClauses = await _parser.ParseClauses(slicedContent, document.LegalDocumentId);
            document.Clauses = parsedClauses.ToList();
            document.Summary = await _parser.GenerateSummary(slicedContent, document.LegalDocumentId);

            await _context.Set<LegalDocument>().AddAsync(document);
            await _context.SaveChangesAsync();

            return (document.Clauses, document.Summary);
        }

        public async Task UpdateDocumentAsync(LegalDocument document)
        {
            var parsedClauses = await _parser.ParseClauses(document.Content, document.LegalDocumentId);
            document.Clauses = parsedClauses.ToList();
            document.Summary = await _parser.GenerateSummary(document.Content, document.LegalDocumentId);

            _context.Set<LegalDocument>().Update(document);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(Guid documentId)
        {
            var document = await GetDocumentByIdAsync(documentId);
            if (document != null)
            {
                _context.Set<LegalDocument>().Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}
