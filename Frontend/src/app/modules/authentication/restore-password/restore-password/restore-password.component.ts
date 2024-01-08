import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { CustomErrorStateMatcher } from '../../../../shared/utils/CustomErrorStateMatcher';
import { RestorePassword } from '../models/RestorePassword';
import { RestorePasswordService } from '../services/restore-password.service';
import { ValidatorService } from 'src/app/shared/services/validators/validator.service';

@Component({
  selector: 'app-restore-password',
  templateUrl: './restore-password.component.html',
  styleUrls: ['./restore-password.component.scss'],
})
export class RestorePasswordComponent implements OnInit {
  constructor(
    private activatedRoute: ActivatedRoute,
    private restorePasswordService: RestorePasswordService,
    private formBuilder: FormBuilder,
    private snackbar: MatSnackBar,
    private router: Router,
    private translate: TranslateService,
    private validatorService: ValidatorService
  ) {}

  isTokenVerified: boolean = false;
  isTokenProvided: boolean = false;
  isLoading: boolean = true;
  token!: string;
  passwordForm!: FormGroup;
  matcher = new CustomErrorStateMatcher();
  hideNew: boolean = true;
  hideConfirm: boolean = true;

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.token = params['remindPasswordToken'];
      if (this.token != null) {
        this.isTokenProvided = true;
        this.restorePasswordService
          .checkToken({ remindPasswordToken: this.token })
          .subscribe({
            next: () => {
              this.isTokenVerified = true;
              this.isLoading = false;
            },
            error: (error) => {
              this.isLoading = false;
              this.isTokenVerified = false;
            },
          });
      } else {
        this.isTokenProvided = false;
        this.isLoading = false;
      }
    });

    this.passwordForm = this.formBuilder.group({
      newPasswordControl: [
        '',
        this.validatorService.getPasswordValidators(),
      ],
      confirmNewPasswordControl: [
        '',
        [Validators.required, this.validatorService.matchValidator('newPasswordControl')],
      ],
    });
  }

  changePassword() {
    let restorePassword: RestorePassword = {
      remindPasswordToken: this.token,
      newPassword: this.passwordForm.get('newPasswordControl')?.value,
      repeatNewPassword: this.passwordForm.get('confirmNewPasswordControl')?.value
    };

    this.restorePasswordService.restorePassword(restorePassword).subscribe({
      next: () => {
        this.router.navigate(['/login']);
        this.translate
          .get('Snackbar.PasswordChanged')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 3000 })
          );
      },
      error: (error) => {
        this.snackbar.open(error.error, 'X', { duration: 3000 });
      },
    });
  }
}
