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
        }
    }
}
