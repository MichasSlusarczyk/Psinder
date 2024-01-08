import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Role, User } from 'src/app/modules/users/models/User';
import { UserService } from 'src/app/modules/users/services/user.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';

@Injectable({
  providedIn: 'root'
})
export class LoggedInUserService {

  constructor(private route: Router, private userService: UserService, private enumsService: EnumsService) { }

  setLoggedInUser(userId: number) {
    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        localStorage.setItem('loggedInUser', JSON.stringify(user));
      },
      error: (error) => {
        this.logout();
      },
    });

  }

  setDifferentSignIn(googleSignIn: boolean, facebookSignIn: boolean) {
    localStorage.setItem('googleSignIn', googleSignIn.toString());
    localStorage.setItem('facebookSignIn', facebookSignIn.toString());
  }

  getIsGoogleSignIn(): boolean {
    let googleSignIn = localStorage.getItem('googleSignIn')
    return googleSignIn !== null && googleSignIn.toLowerCase() === "true"
  }

  getIsFacebookSignIn(): boolean {
    let facebookSignIn = localStorage.getItem('facebookSignIn')
    return facebookSignIn !== null && facebookSignIn.toLowerCase() === "true"
  }

  logout() {
    localStorage.clear()
    localStorage.setItem('language', 'en');
    this.route.navigate(['/login'])
  }

  getLoggedInUser(): User {
    return localStorage.getItem('loggedInUser') ? JSON.parse(localStorage.getItem('loggedInUser')!) : null;
  }

  setAuthorizationHeader(accessToken: string, refreshToken: string) {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
  }

  setAccessToken(accessToken: string) {
    localStorage.setItem('accessToken', accessToken);
  }

  getUserPermission() {
    return localStorage.getItem('loggedInUser') ? this.enumsService.getRoleFromOrdinal(
      JSON.parse(localStorage.getItem('loggedInUser')!).role) : null;
  }

  isAdminLoggedIn() {
    return this.getUserPermission() === Role.ADMIN;
  }

  isWorkerLoggedIn() {
    return this.getUserPermission() === Role.ADMIN || this.getUserPermission() === Role.WORKER;
  }

  isOnlyWorkerLoggedIn() {
    return this.getUserPermission() === Role.WORKER;
  }

  isOnlyUserLoggedIn() {
    return this.getUserPermission() === Role.USER
  }

  isUserLoggedIn() {
    return this.getUserPermission() === Role.ADMIN || this.getUserPermission() === Role.USER;
  }

  changeUsedLanguage(language: string) {
    localStorage.setItem('language', language);
  }

  getUsedLanguage() {
    return localStorage.getItem('language');
  }
}
