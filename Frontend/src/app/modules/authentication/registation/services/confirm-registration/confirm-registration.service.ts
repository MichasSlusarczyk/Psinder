import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { VerificationToken } from '../../../login/models/VerificationToken';

@Injectable({
  providedIn: 'root'
})
export class ConfirmRegistrationService {

  private url: string = `${environment.apiUrl}/registration/verify`;

  constructor(private http: HttpClient) { }

  confirmRegistration(verificationToken: VerificationToken): Observable<null> {
    return this.http.post<null>(this.url, verificationToken);
  }
}
