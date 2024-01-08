import {
  Component,
  NgZone,
  OnInit,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { User } from '../../../users/models/User';
import { CustomErrorStateMatcher } from '../../../../shared/utils/CustomErrorStateMatcher';
import { LoginDto } from '../models/Login';
import { LoggedInUserService } from '../services/logged-in-user/logged-in-user.service';
import { LoginService } from '../services/login/login.service';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { environment } from 'src/environments/environment';
import { LoginResponse } from '../models/LoginResponse';

declare var grecaptcha: any;
declare const FB: any;

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss'],
})
export class LoginFormComponent implements OnInit {
  user!: User;
  form!: FormGroup;
  isError: boolean = false;
  recaptchaForm!: FormGroup;
  matcher = new CustomErrorStateMatcher();
  errorMessage: string = 'Email or password is incorrect. Please try again.';
  isLoading: boolean = false;
  hide: boolean = true;

  constructor(
    private formBuilder: FormBuilder,
    private loginService: LoginService,
    private router: Router,
    private loggedInUserService: LoggedInUserService,
    private translate: TranslateService,
    private ngZone: NgZone
  ) {
  }

  ngOnInit() {
    this.hide = true;
    this.isLoading = false;
    this.form = this.formBuilder.group({
      usernameControl: ['', Validators.required],
      passwordControl: ['', Validators.required],
    });

    this.recaptchaForm = this.formBuilder.group({
      captchaFormControl: ['', Validators.required],
    });
  }

  ngAfterViewInit() {
    this.ngZone.run(() => {
      this.initializeGoogleSignIn();
    });
  }

  initializeGoogleSignIn() {
    // @ts-ignore
    //window.onGoogleLibraryLoad = () => {
    window.onload = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: environment.googleClientId,
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });
      // @ts-ignore
      google.accounts.id.renderButton(
        // @ts-ignore
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large"}
      );
      // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => { });
    };
  }

  handleCredentialResponse(response: CredentialResponse) {
    this.loginService.loginGoogle({ googleToken: response.credential }).subscribe({
      next: (loginResponse) => {
        if(!!loginResponse.status && loginResponse.status === 1) {
          this.loggedInUserService.setDifferentSignIn(true, false);
          this.handleSuccessfullSignInOther(loginResponse)
        } else {
          this.handleErrorSignIn('The email or password is incorrect')
        }
      },
      error: (error) => {
        this.handleErrorSignIn(error.error)
      },
    });
  }

  loginFacebook() {
    FB.login(async (result: any) => {
      this.loginService.loginFacebook({ facebookToken: result.authResponse.accessToken }).subscribe({
        next: (loginResponse) => {
          if(!!loginResponse.status && loginResponse.status === 1) {
            this.loggedInUserService.setDifferentSignIn(false, true);
            this.handleSuccessfullSignInOther(loginResponse)
          } else {
            this.handleErrorSignIn('The email or password is incorrect')
          }
        },
        error: (error) => {
          this.handleErrorSignIn(error.error)
        },
      });
    }, { scope: 'email,public_profile' });

  }

  login() {
    this.isLoading = true;

    const response = grecaptcha.getResponse();
    if (response.length === 0) {
      this.isLoading = false;
      this.isError = true;
      this.translate.get('Login.RecaptchaMessage').subscribe((message: string) => this.errorMessage = message);

      return;
    }

    let login: LoginDto = {
      login: this.form.get('usernameControl')?.value,
      password: this.form.get('passwordControl')?.value,
      captchaResponse: response,
    };
    this.loginService.login(login).subscribe({
      next: (loginResponse) => {
        if(!!loginResponse.status && loginResponse.status === 1) {
          this.handleSuccessfullSignIn(loginResponse)
        } else {
          this.handleErrorSignIn('The email or password is incorrect')
        }
      },
      error: (error) => {
        this.handleErrorSignIn(error.error)
      },
    });
  }

  handleSuccessfullSignIn(loginResponse: LoginResponse) {
    this.loggedInUserService.setAuthorizationHeader(
      loginResponse.accessToken,
      loginResponse.refreshToken,
    );
    this.loggedInUserService.setLoggedInUser(loginResponse.userId);
    this.isLoading = false;
    this.router.navigate(['/pets']);
  }

  handleSuccessfullSignInOther(loginResponse: LoginResponse) {
    this.loggedInUserService.setAuthorizationHeader(
      loginResponse.accessToken,
      loginResponse.refreshToken,
    );
    this.loggedInUserService.setLoggedInUser(loginResponse.userId);
    this.isLoading = false;
    
    this.ngZone.run(() => {
      this.router.navigate(['/pets']);
    });
  }

  handleErrorSignIn(error: string) {
    this.isLoading = false;
    this.form.get('usernameControl')?.setValue('');
    this.form.get('passwordControl')?.setValue('');
    this.isError = true;
    this.errorMessage = error;
  }
}
