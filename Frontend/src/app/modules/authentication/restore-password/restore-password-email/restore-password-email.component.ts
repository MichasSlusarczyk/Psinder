import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { CustomErrorStateMatcher } from '../../../../shared/utils/CustomErrorStateMatcher';
import { Email } from '../models/Email';
import { RestorePasswordService } from '../services/restore-password.service';

@Component({
  selector: 'app-restore-password-email',
  templateUrl: './restore-password-email.component.html',
  styleUrls: ['./restore-password-email.component.scss'],
})
export class RestorePasswordEmailComponent implements OnInit {
  form!: FormGroup;
  matcher = new CustomErrorStateMatcher();
  isLoading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private snackbar: MatSnackBar,
    private restorePasswordService: RestorePasswordService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.isLoading = false;
    this.form = this.formBuilder.group({
      emailControl: ['', [Validators.required, Validators.email]],
    });
  }

  sendEmail() {
    let email: Email = {
      login: this.form.get('emailControl')?.value,
    };
    this.restorePasswordService.sendEmail(email).subscribe({
      next: () => {
        this.translate
          .get('Snackbar.WeSendYouEmail')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 4000 })
          );
      },
      error: (error) => {
        this.translate
          .get('Snackbar.EmailNotExists')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 3000 })
          );
      },
    });
  }
}
