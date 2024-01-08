import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoggedInUserService } from 'src/app/modules/authentication/login/services/logged-in-user/logged-in-user.service';
import { Role } from 'src/app/modules/users/models/User';

@Injectable({
  providedIn: 'root'
})
export class WorkerGuard implements CanActivate {
  constructor(
    private loggedInUserService: LoggedInUserService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (
        this.loggedInUserService.getUserPermission() &&
        this.loggedInUserService.getUserPermission() === Role.WORKER ||
        this.loggedInUserService.getUserPermission() == Role.ADMIN
      ) {
        return true;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
  }
  
}
