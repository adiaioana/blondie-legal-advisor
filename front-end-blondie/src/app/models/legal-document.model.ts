export interface LegalDocument {
  legalDocumentId: string;
  fileName: string;
  uploadedAt: string;
  // Optionally for detail page:
  clauses?: Clause[];
  summary?: DocumentSummary;
}
export interface Clause {
  clauseId: string;
  text: string;
}
export interface DocumentSummary {
  content: string;
}
