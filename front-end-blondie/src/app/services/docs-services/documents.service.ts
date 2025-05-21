import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LegalDocument } from '../../models/legal-document.model';
import {environment} from '../../envinronment';

@Injectable({ providedIn: 'root' })
export class DocumentsService {
  private apiUrl  = environment.apiUrl +'/LegalDocuments';

  constructor(private http: HttpClient) {}

  getDocuments(): Observable<LegalDocument[]> {
    return this.http.get<LegalDocument[]>(this.apiUrl);
  }

  getDocumentById(id: string): Observable<LegalDocument> {
    return this.http.get<LegalDocument>(`${this.apiUrl}/${id}`);
  }
}
