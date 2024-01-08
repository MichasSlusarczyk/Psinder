import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/modules/users/models/User';
import { RegistrationForm } from '../../model/RegistrationForm';
import { RegisterResponse } from '../../model/RegisterResponse';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private url: string = `${environment.apiUrl}/registration/register`;

  constructor(private http: HttpClient) { }

  register(registrationForm: RegistrationForm): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(this.url, registrationForm);
  }
}
