import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  BehaviorSubject,
  catchError,
  filter,
  Observable,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { LogoutDialogComponent } from '../../modules/authentication/login/logout-dialog/logout-dialog.component';
import { AccessTokenDto, RefreshTokenDto } from '../../modules/authentication/login/models/Token';
import { LoggedInUserService } from '../../modules/authentication/login/services/logged-in-user/logged-in-user.service';
import { LoginService } from '../../modules/authentication/login/services/login/login.service';

@Injectable({
  providedIn: 'root',
})
export class HttpInterceptorService implements HttpInterceptor {
  constructor(
    private loggedInUser: LoggedInUserService,
    private loginService: LoginService,
    private dialog: MatDialog
  ) {}

  private isRefreshing: boolean = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<Object>> {
    this.isRefreshing = false;
    let authReq = req;
    if (localStorage.getItem('accessToken')) {
      authReq = this.addTokenHeader(
        req,
        localStorage.getItem('accessToken')!
      );

      return next.handle(authReq).pipe(
        catchError((error) => {
          if (
            error instanceof HttpErrorResponse &&
            authReq.url.includes('psinder') &&
            !authReq.url.includes('login') &&
            (error.status === 401 || error.status === 403)
          ) {
            console.log('Error:' + error)
            return this.handleAuthorizationError(authReq, next);
          }
          return next.handle(authReq)
          //return throwError(() => new Error());
        })
      );
    } else {
      return next.handle(req);
    }
  }

  private handleAuthorizationError(
    request: HttpRequest<any>,
    next: HttpHandler
  ) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      if (localStorage.getItem('refreshToken')) {
        let refreshTokenDto: RefreshTokenDto = {
          refreshToken: localStorage.getItem('refreshToken')!,
          userId: this.loggedInUser.getLoggedInUser().id!
        };

        return this.loginService
          .getNewAccessTokenFromRefreshToken(refreshTokenDto)
          .pipe(
            catchError((err) => {
              this.isRefreshing = false;

              const dialogRef = this.dialog.open(LogoutDialogComponent, {
                width: '250px',
              });
              dialogRef.afterClosed().subscribe((result) => {
                this.loggedInUser.logout();
              });

              return throwError(() => new Error());
            }),
            switchMap((token: AccessTokenDto) => {
              this.isRefreshing = false;

              this.loggedInUser.setAccessToken(token.accessToken);
              this.refreshTokenSubject.next(token.accessToken);

              return next.handle(
                this.addTokenHeader(request, token.accessToken)
              );
            })
          );
      }
    }

    return this.refreshTokenSubject.pipe(
      filter((token) => token !== null),
      take(1),
      switchMap((token) => next.handle(this.addTokenHeader(request, token)))
    );
  }

  private addTokenHeader(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: 'Bearer ' + token,
      },
    });
  }
}
