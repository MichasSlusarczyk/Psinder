import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { LoggedInUserService } from '../../../modules/authentication/login/services/logged-in-user/logged-in-user.service';

@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {
  constructor(
    private loggedInUser: LoggedInUserService,
    private router: Router,
    private snackbar: MatSnackBar,
    private translate: TranslateService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.loggedInUser.getLoggedInUser() !== null) {
      this.router.navigate(['/pets']);
      this.translate
        .get('Snackbar.YouAreSignedIn')
        .subscribe((message: string) =>
          this.snackbar.open(message, 'X', { duration: 3000 })
        );
      return false;
    } else {
      return true;
    }
  }
}
