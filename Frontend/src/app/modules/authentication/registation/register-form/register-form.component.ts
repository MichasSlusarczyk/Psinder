import { KeyValue } from '@angular/common';
import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LoggedInUserService } from '../../login/services/logged-in-user/logged-in-user.service';
import { CustomErrorStateMatcher } from '../../../../shared/utils/CustomErrorStateMatcher';
import { getLocale } from '../../../../shared/utils/GetLocale';
import { PersonalData, PersonGender } from '../../../users/models/PersonalData';
import { Role } from '../../../users/models/User';
import { getFormattedDate } from 'src/app/shared/utils/GetFormattedDate';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { ValidatorService } from 'src/app/shared/services/validators/validator.service';
import { RegisterService } from '../services/register/register.service';
import { RegistrationForm } from '../model/RegistrationForm';
import { Shelter } from 'src/app/modules/shelter/models/Shelter';
import { ShelterService } from 'src/app/modules/shelter/services/shelter.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  @ViewChild('container', { read: ViewContainerRef, static: true })
  private container!: ViewContainerRef;

  originalOrder = (
    a: KeyValue<string, PersonGender>,
    b: KeyValue<string, PersonGender>
  ): number => {
    return 0;
  };

  constructor(
    private formBuilder: FormBuilder,
    private loggedInUserService: LoggedInUserService,
    private router: Router,
    private adapter: DateAdapter<any>,
    private snackbar: MatSnackBar,
    private translate: TranslateService,
    private enumsService: EnumsService,
    private validatorService: ValidatorService,
    private registerService: RegisterService,
    private shelterService: ShelterService
  ) { }

  form!: FormGroup;
  personalDataForm!: FormGroup;
  matcher = new CustomErrorStateMatcher();

  hide: boolean = true;
  hideConfirm: boolean = true;
  maxDate: Date = new Date();

  shelters: Shelter[] = []

  isError: boolean = false;
  errorMessage: string = 'Email is already in use.';

  ngOnInit(): void {
    this.adapter.setLocale(getLocale());

    this.form = this.formBuilder.group({
      passwordControl: [
        '',
        this.validatorService.getPasswordValidators(),
      ],
      emailControl: ['', [Validators.required, Validators.email]],
      confirmPasswordControl: [
        '',
        [Validators.required, this.validatorService.matchValidator('passwordControl')],
      ],
      roleControl: [Role.USER],
      shelterControl: []
    });
    this.personalDataForm = this.formBuilder.group({
      firstNameControl: ['', Validators.required],
      lastNameControl: ['', Validators.required],
      phoneNumberControl: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      cityControl: ['', Validators.required],
      postalCodeControl: [''],
      streetControl: [''],
      streetNumberControl: [''],
      genderControl: [PersonGender.OTHER, Validators.required],
      birthDateControl: ['', Validators.required],
      PostletterControl: [false]
    });

    this.getShelters();
  }

  getRole() {
    return this.enumsService.getRole();
  }

  getPersonGender() {
    return this.enumsService.getPersonGender();
  }

  onSubmit() {
    const formattedDate = getFormattedDate(
      this.personalDataForm.get('birthDateControl')?.value
    );

    let personalData: PersonalData = {
      firstName: this.personalDataForm.get('firstNameControl')?.value,
      lastName: this.personalDataForm.get('lastNameControl')?.value,
      phoneNumber: this.personalDataForm.get('phoneNumberControl')?.value,
      city: this.personalDataForm.get('cityControl')?.value,
      street: this.personalDataForm.get('streetControl')?.value,
      streetNumber: this.personalDataForm.get('streetNumberControl')?.value,
      gender: this.enumsService.getPersonGenderOrdinal(this.personalDataForm.get('genderControl')?.value),
      postalCode: this.personalDataForm.get('postalCodeControl')?.value,
      birthDate: formattedDate,
    };

    let registrationForm: RegistrationForm = {
      userDetails: personalData,
      email: this.form.get('emailControl')?.value,
      password: this.form.get('passwordControl')?.value,
      repeatPassword: this.form.get('confirmPasswordControl')?.value,
      role: this.form.get('roleControl')?.value != null ?
        this.enumsService.getRoleOrdinal(this.form.get('roleControl')?.value) : 2,
      shelterId: this.form.get('shelterControl')?.value
    };

    let id = this.form.get('shelterControl')?.value
    registrationForm.shelterId = !!id ? parseInt(id) : undefined

    this.registerService.register(registrationForm).subscribe({
      next: (response) => {
        if (!!response.status && (response.status === 11 || response.status === 13)) {
          this.isError = true;
        } else {
          if (this.isAdminLoggedIn()) {
            this.router.navigate(['/users']);
          } else {
            this.router.navigate(['/login']);
          }
          this.translate
            .get('Snackbar.AccountCreated')
            .subscribe((message: string) =>
              this.snackbar.open(message, 'X', { duration: 6000 })
            );
        }
      },
      error: (error) => {
        this.isError = true;
        this.errorMessage = error.error;
      },
    });
  }

  isAdminLoggedIn(): any {
    this.loggedInUserService.getUserPermission();
    return this.loggedInUserService.getUserPermission() === Role.ADMIN;
  }

  getShelters() {
    this.shelterService.getShelters().subscribe({
      next: (shelters) => {
        this.shelters = shelters.shelters
      },
      error: (error) => {
        console.log(error)
        this.shelters = []
      }
    })
  }

  isWorkerSelected() {
    return this.form.get('roleControl')?.value != null &&
      this.form.get('roleControl')?.value == Role.WORKER
  }
}
