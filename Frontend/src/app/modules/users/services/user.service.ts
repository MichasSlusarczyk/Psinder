import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChangePassword, User, UserResponse } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private url = `${environment.apiUrl}/user`;

  constructor(private http: HttpClient) {}

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.url}/${id}`);
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(this.url, user);
  }

  blockUser(id: number): Observable<null> {
    return this.http.delete<null>(`${this.url}/${id}/block`);
  }

  addUser(user: User): Observable<User> {
    return this.http.post<User>(this.url, user);
  }

  deleteUser(id: number): Observable<null> {
    return this.http.delete<null>(`${this.url}/${id}/deactivate`);
  }

  getUsers(): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.url}/all`);
  }
}
