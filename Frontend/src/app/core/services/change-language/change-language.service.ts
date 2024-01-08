import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Language } from '../../models/Language';

@Injectable({
  providedIn: 'root'
})
export class ChangeLanguageService {

  private url = `${environment.apiUrl}/translate`;

  constructor(private http: HttpClient) {}

  changeLanguage(language: Language): Observable<null> {
    let params = new HttpParams().set('language', language);

    return this.http.get<null>(this.url, { params: params });
  }
}
