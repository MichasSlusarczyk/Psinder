import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmRegistrationService } from '../services/confirm-registration/confirm-registration.service';

@Component({
  selector: 'app-confirm-registration',
  templateUrl: './confirm-registration.component.html',
  styleUrls: ['./confirm-registration.component.scss']
})
export class ConfirmRegistrationComponent implements OnInit {

  message!: string
  isLoading: boolean = true;
  isTokenVerified: boolean = false;

  constructor(private activatedRoute: ActivatedRoute,
              private confirmRegistrationService: ConfirmRegistrationService,
              private translate: TranslateService) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const token = params['registerVerificationToken'];
      if(token != null) {
        this.confirmRegistrationService.confirmRegistration({registerVerificationToken: token}).subscribe({next: () => {
          this.isTokenVerified = true;
          this.isLoading = false;
          this.translate.get('ActivatedAccount.Message').subscribe((message:string) => this.message = message);
        },
        error: (error) => {
          this.isLoading = false;
          this.message = error.error;
        }
      })
      } else {
        this.isLoading = false;
        this.translate.get('ActivatedAccount.ErrorMessage').subscribe((message:string) => this.message = message);
      }
    });
  }

}
