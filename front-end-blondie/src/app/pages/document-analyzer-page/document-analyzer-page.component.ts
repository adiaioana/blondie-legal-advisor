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
  clauses: string = '';
  summary: string = '';

  constructor(private legalDocumentsService: LegalDocumentsService) {
    // Initialize with default values or fetch data dynamically
    this.clauses = 'Default clauses content';
    this.summary = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sed odio non augue euismod faucibus vulputate viverra massa. Sed et elit mattis, fringilla justo nec, semper arcu. Phasellus nibh ipsum, volutpat ac suscipit nec, faucibus non justo. Proin elit nisl, euismod ut imperdiet et, bibendum eget orci. Vestibulum vulputate ante augue, id efficitur augue vehicula vel. Ut orci nibh, porta id libero quis, porta lacinia odio. Aliquam in iaculis ante, vel molestie nibh. Cras quis ligula quis sapien tempus fermentum. Vestibulum porta dui ac lacus aliquet pharetra. Integer consectetur posuere enim, in iaculis orci pulvinar nec. Vivamus sed posuere orci. Nam a porta lorem. Duis eget enim vulputate, iaculis ante at, iaculis diam. Quisque egestas, metus et mattis congue, est nisl laoreet nunc, vel elementum orci metus a eros.";
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
