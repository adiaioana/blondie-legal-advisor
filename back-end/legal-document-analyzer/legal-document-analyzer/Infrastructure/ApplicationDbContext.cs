using legal_document_analyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace legal_document_analyzer.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LegalDocument> LegalDocuments { get; set; }
        public DbSet<Clause> Clauses { get; set; }
        public DbSet<DocumentSummary> DocumentSummaries { get; set; }
        public DbSet<ChatSession> ChatSessions {get; set;}
        public DbSet<MyChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.Property(e => e.Username).HasColumnName("username");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
                entity.Property(e => e.PasswordSalt).HasColumnName("passwordsalt");
                entity.Property(e => e.RefreshToken).HasColumnName("refreshtoken");
            });

            modelBuilder.Entity<LegalDocument>(entity =>
            {
                entity.ToTable("legaldocuments");
                entity.HasKey(e => e.LegalDocumentId);
                entity.Property(e => e.LegalDocumentId).HasColumnName("legaldocumentid");
                entity.Property(e => e.FileName).HasColumnName("filename");
                entity.Property(e => e.Content).HasColumnName("content").HasColumnType("TEXT");
                entity.Property(e => e.UploadedAt).HasColumnName("uploadedat");
                entity.Property(e => e.UserId).HasColumnName("userid");

                entity.HasMany(e => e.Clauses)
                      .WithOne()
                      .HasForeignKey(c => c.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Summary)
                      .WithOne()
                      .HasForeignKey<DocumentSummary>(ds => ds.DocumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Clause>(entity =>
            {
                entity.ToTable("clauses");
                entity.HasKey(e => e.ClauseId);
                entity.Property(e => e.ClauseId).HasColumnName("clauseid");
                entity.Property(e => e.DocumentId).HasColumnName("documentid");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.Explanation).HasColumnName("explanation");
            });

            modelBuilder.Entity<DocumentSummary>(entity =>
            {
                entity.ToTable("documentsummaries");
                entity.HasKey(e => e.DocumentSummaryId);
                entity.Property(e => e.DocumentSummaryId).HasColumnName("documentsummaryid");
                entity.Property(e => e.DocumentId).HasColumnName("documentid");
                entity.Property(e => e.Style).HasColumnName("style");
                entity.Property(e => e.Content).HasColumnName("content");
            });

            modelBuilder.Entity<ChatSession>(entity =>
            {
                // Table Name
                entity.ToTable("chatsessions");

                // Primary Key
                entity.HasKey(e => e.Id);

                // Property Mappings
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.IsActive).HasColumnName("isactive");

                // Relationships
                entity.HasMany(e => e.Messages)
                      .WithOne(cm => cm.ChatSession)
                      .HasForeignKey(cm => cm.ChatSessionId);
            });

            // ChatMessage Configuration
            modelBuilder.Entity<MyChatMessage>(entity =>
            {
                entity.ToTable("chatmessages");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ChatSessionId).HasColumnName("chatsessionid");
                entity.Property(e => e.Sender).HasColumnName("sender");
                entity.Property(e => e.Content).HasColumnName("content");
                entity.Property(e => e.InputMode).HasColumnName("inputmode");
                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.HasOne(cm => cm.ChatSession)
                      .WithMany(cs => cs.Messages)
                      .HasForeignKey(cm => cm.ChatSessionId);
            });
        }
    }
}
