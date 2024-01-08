import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoggedInUserService } from '../../../modules/authentication/login/services/logged-in-user/logged-in-user.service';
import { Role } from '../../../modules/users/models/User';

@Injectable({
  providedIn: 'root'
})
export class UserGuard implements CanActivate {
  constructor(
    private loggedInUserService: LoggedInUserService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (
      this.loggedInUserService.getUserPermission() &&
      (this.loggedInUserService.getUserPermission() === Role.USER || 
      this.loggedInUserService.getUserPermission() === Role.ADMIN ||
      this.loggedInUserService.getUserPermission() == Role.WORKER)
    ) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
