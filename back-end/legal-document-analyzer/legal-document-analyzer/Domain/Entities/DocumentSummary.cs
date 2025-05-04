using legal_document_analyzer.Domain.ValueObjects;

namespace legal_document_analyzer.Domain.Entities
{
    public class DocumentSummary
    {
        public Guid DocumentSummaryId { get; set; }
        public Guid DocumentId { get; set; }
        public SummaryStyle Style { get; set; }
        public string Content { get; set; }
    }
}
