import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AttachmentService {

  private url: string = `${environment.apiUrl}/file`;

  constructor(private http: HttpClient) { }

  getImage(id: number): Observable<Blob> {
    return this.http.get(`${this.url}/${id}/unauthorized`, { responseType: 'blob' });
  }
}
