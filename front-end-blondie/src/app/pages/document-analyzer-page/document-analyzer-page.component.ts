import {Component, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {LegalDocumentsService} from '../../services/file-services/legal-documents.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-document-analyzer-page',
  imports: [CommonModule],
  templateUrl: './document-analyzer-page.component.html',
  styleUrl: './document-analyzer-page.component.css',
  standalone: true
})
export class DocumentAnalyzerPageComponent {
  clauses: string = '';
  summary: string = '';
  loading: boolean = false;
  resultsLoaded: boolean = false;

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

      this.loading = true;
      this.resultsLoaded = false;
      this.legalDocumentsService.uploadDocument(file).subscribe({
        next: (response) => {
          //console.log('Full response:', response);
          const legalDocumentId = response.legalDocumentId;
          const fileName = response.fileName;
          this.clauses = this.convertMarkdownToHtml(response.clauseContents.$values?.join('\n\n') || '');
          this.summary = this.convertMarkdownToHtml(response.content || '');


          console.log('I do not understand');
          console.log('Clauses:', this.clauses);
          console.log('Summary:', this.summary);
          this.loading = false;
          this.resultsLoaded = true;
        },
        error: (err) => {
          console.error('Upload failed', err);
          this.loading = false;
        }
      });

    }
  }
  convertMarkdownToHtml(markdown: string): string {
    let html = markdown;

    // Headers
    html = html.replace(/^### (.*$)/gim, '<h3>$1</h3>');
    html = html.replace(/^## (.*$)/gim, '<h2>$1</h2>');
    html = html.replace(/^# (.*$)/gim, '<h1>$1</h1>');

    // Bold
    html = html.replace(/\*\*(.*?)\*\*/gim, '<strong>$1</strong>');

    // Italic
    html = html.replace(/\*(.*?)\*/gim, '<em>$1</em>');

    // Unordered list items
    html = html.replace(/^- (.*$)/gim, '<li>$1</li>');
    html = html.replace(/(<li>.*<\/li>)/gim, '<ul>$1</ul>');

    // Line breaks
    html = html.replace(/\n/gim, '<br/>');

    return html.trim();
  }


}
