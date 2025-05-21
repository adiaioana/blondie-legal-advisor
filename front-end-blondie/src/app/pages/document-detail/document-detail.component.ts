import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import { DocumentsService } from '../../services/docs-services/documents.service';
import { LegalDocument } from '../../models/legal-document.model';
import {CommonModule, DatePipe} from '@angular/common';

@Component({
  selector: 'app-document-detail',
  templateUrl: './document-detail.component.html',
  imports: [
    DatePipe,
    CommonModule
  ],
  styleUrl: 'document-detail.component.css',
  standalone: true
})
export class DocumentDetailComponent implements OnInit {
  document?: LegalDocument;
  isLoading = true;

  constructor(private route: ActivatedRoute, private documentsService: DocumentsService, private router:Router) {}

  goBack(): void {
    this.router.navigate(['/documents']);
  }
  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.documentsService.getDocumentById(id).subscribe(doc => {
      this.document = doc;
      this.isLoading = false;
    });
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
