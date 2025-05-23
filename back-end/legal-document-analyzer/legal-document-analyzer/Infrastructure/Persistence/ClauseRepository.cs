using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace legal_document_analyzer.Infrastructure.Persistence
{
    public class ClauseRepository : IClauseRepository
    {
        private readonly ApplicationDbContext _context;

        public ClauseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Clause> GetClauseByIdAsync(Guid clauseId)
        {
            return await _context.Clauses
                .FirstOrDefaultAsync(c => c.ClauseId == clauseId);
        }

        public async Task<IEnumerable<Clause>> GetClausesByDocumentIdAsync(Guid documentId)
        {
            return await _context.Clauses
                .Where(c => c.DocumentId == documentId)
                .ToListAsync();
        }

        public async Task AddClauseAsync(Clause clause)
        {
            await _context.Clauses.AddAsync(clause);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClauseAsync(Clause clause)
        {
            _context.Clauses.Update(clause);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClauseAsync(Guid clauseId)
        {
            var clause = await _context.Clauses.FindAsync(clauseId);
            if (clause != null)
            {
                _context.Clauses.Remove(clause);
                await _context.SaveChangesAsync();
            }
        }
    }
}

