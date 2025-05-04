using legal_document_analyzer.Domain.ValueObjects;

namespace legal_document_analyzer.Domain.Entities
{
    public class Clause
    {
        public Guid ClauseId { get; set; }
        public Guid DocumentId { get; set; }
        public ClauseType Type { get; set; }
        public string Text { get; set; }
        public string Explanation { get; set; } // LLM explanation
    }
}
