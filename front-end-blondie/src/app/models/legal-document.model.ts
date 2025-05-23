export interface LegalDocument {
  legalDocumentId: string;
  fileName: string;
  uploadedAt: string;
  // Optionally for detail page:
  clauses?: Horror;
  summary?: DocumentSummary;
}
export interface Horror{
  $id: any;
  $values: Clause[];
}
export interface Clause {
  clauseId: string;
  text: string;

}
export interface DocumentSummary {
  content: string;
}
