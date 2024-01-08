import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Email } from '../models/Email';
import { RestorePassword } from '../models/RestorePassword';
import { RestorePasswordToken } from '../models/RestorePasswordToken';

@Injectable({
  providedIn: 'root',
})
export class RestorePasswordService {
  private url: string = `${environment.apiUrl}/login`;

  constructor(private http: HttpClient) {}

  sendEmail(email: Email): Observable<null> {
    return this.http.post<null>(`${this.url}/send-password-reminder`, email);
  }

  checkToken(token: RestorePasswordToken): Observable<null> {
    return this.http.post<null>(`${this.url}/remind-password`, token);
  }

  restorePassword(restorePassword: RestorePassword): Observable<null> {
    return this.http.post<null>(`${this.url}/reset-password`, restorePassword);
  }
}
