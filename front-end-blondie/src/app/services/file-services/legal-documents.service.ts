import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../envinronment';

@Injectable({
  providedIn: 'root'
})
export class LegalDocumentsService {
  private baseUrl = environment.apiUrl;
  private uploadUrl = this.baseUrl+ '/LegalDocuments/upload';

  constructor(private http: HttpClient) {}

  uploadDocument(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(this.uploadUrl, formData);
  }

}
