using legal_document_analyzer.Domain.Entities;
using legal_document_analyzer.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace legal_document_analyzer.Infrastructure.Persistence
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatSession?> GetActiveSessionAsync(string userId)
        {
            return await _context.ChatSessions
                .Include(cs => cs.Messages)
                .FirstOrDefaultAsync(cs => cs.UserId == userId && cs.IsActive);
        }

        public async Task<ChatSession> CreateSessionAsync(string userId)
        {
            var existing = await GetActiveSessionAsync(userId);
            if (existing != null) throw new InvalidOperationException("User already has an active session.");

            var session = new ChatSession { UserId = userId };
            _context.ChatSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task EndSessionAsync(Guid sessionId)
        {
            var session = await _context.ChatSessions.FindAsync(sessionId);
            if (session != null)
            {
                session.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddMessageAsync(Guid sessionId, string sender, string content, string inputMode)
        {
            var message = new MyChatMessage
            {
                ChatSessionId = sessionId,
                Sender = sender,
                Content = content,
                InputMode = inputMode
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MyChatMessage>> GetMessagesAsync(Guid sessionId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatSessionId == sessionId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }

}
