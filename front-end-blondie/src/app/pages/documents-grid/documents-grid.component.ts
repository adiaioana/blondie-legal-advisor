import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import { DocumentsService } from '../../services/docs-services/documents.service';
import { LegalDocument } from '../../models/legal-document.model';
import { Router } from '@angular/router';
import {CommonModule, DatePipe} from '@angular/common';

@Component({
  selector: 'app-documents-grid',
  templateUrl: './documents-grid.component.html',
  imports: [
    DatePipe,
    CommonModule
  ],
  styleUrl: 'documents-grid.component.css',
  standalone: true,
  encapsulation: ViewEncapsulation.None
})
export class DocumentsGridComponent implements OnInit {
  documents: LegalDocument[] = [];
  isLoading = true;
  error: string | null = null;
  constructor(private documentsService: DocumentsService, private router: Router) {}

  ngOnInit() {
    this.documentsService.getDocuments().subscribe({
      next: (docs) => {
        console.log('Documents received:', docs); // <-- Debug output
        this.documents = docs;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading documents:', err); // <-- Debug output
        this.error = 'Failed to load documents';
        this.isLoading = false;
      }
    });
    console.log('DocumentsGridComponent initialized');
  }
  goToDetail(id: string, event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }
    this.router.navigate(['/documents', id]);
  }
  getSummaryPreview(summaryContent?: string, maxLength: number = 200): string {
    if (!summaryContent) return '';
    return summaryContent.length > maxLength
      ? summaryContent.slice(0, maxLength).trim() + '…'
      : summaryContent;
  }
}
