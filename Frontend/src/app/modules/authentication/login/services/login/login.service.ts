import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginDto } from '../../models/Login';
import { LoginResponse } from '../../models/LoginResponse';
import { RefreshTokenDto, AccessTokenDto } from '../../models/Token';
import { LoginGoogleRequest } from '../../models/LoginGoogleRequest';
import { ChangePassword } from 'src/app/modules/users/models/User';
import { LoginFacebookRequest } from '../../models/LoginFacebookRequest';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private url: string = `${environment.apiUrl}/login`;

  constructor(private http: HttpClient) { }

  login(loginDto: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.url, loginDto);
  }

  loginGoogle(loginDto: LoginGoogleRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.url}/google`, loginDto);
  }

  loginFacebook(loginDto: LoginFacebookRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.url}/facebook`, loginDto);
  }

  getNewAccessTokenFromRefreshToken(refreshToken: RefreshTokenDto): Observable<AccessTokenDto> {
    return this.http.post<AccessTokenDto>(`${this.url}/refresh`, refreshToken);
  }

  changePassword(changePassword: ChangePassword): Observable<void> {
    return this.http.post<void>(`${this.url}/change-password`, changePassword);
  }
}
