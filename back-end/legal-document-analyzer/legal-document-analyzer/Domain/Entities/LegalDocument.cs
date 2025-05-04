namespace legal_document_analyzer.Domain.Entities
{
    public class LegalDocument
    {
        public Guid LegalDocumentId { get; set; }
        public Guid UserId { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public DateTime UploadedAt { get; set; }

        public ICollection<Clause> Clauses { get; set; }
        public DocumentSummary Summary { get; set; }

    }
}
