import {Component, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {LegalDocumentsService} from '../../services/file-services/legal-documents.service';

@Component({
  selector: 'app-document-analyzer-page',
  imports: [],
  templateUrl: './document-analyzer-page.component.html',
  styleUrl: './document-analyzer-page.component.css',
  standalone: true
})
export class DocumentAnalyzerPageComponent {

  constructor(private legalDocumentsService: LegalDocumentsService) {
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files!=null && input.files.length > 0) {
      let file = input.files?.[0];
      if (file.type !== 'application/pdf') {
        alert('Only PDF files are allowed.');
        return;
      }

      this.legalDocumentsService.uploadDocument(file).subscribe({
        next: (res) => {
          console.log('Upload successful:', res);
          // TODO: Show summary or ID
        },
        error: (err) => {
          console.error('Upload failed:', err);
        }
      });
    }
  }

}
