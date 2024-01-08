import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DialogBodyComponent } from '../../../shared/components/dialog-body/dialog-body.component';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { CustomErrorStateMatcher } from '../../../shared/utils/CustomErrorStateMatcher';
import { getLocale } from '../../../shared/utils/GetLocale';
import { PersonalData } from '../models/PersonalData';
import { ChangePassword, Role, User } from '../models/User';
import { UserService } from '../services/user.service';
import { getFormattedDate } from 'src/app/shared/utils/GetFormattedDate';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { ValidatorService } from 'src/app/shared/services/validators/validator.service';
import { LoginService } from '../../authentication/login/services/login/login.service';
import { ShelterService } from '../../shelter/services/shelter.service';
import { Shelter } from '../../shelter/models/Shelter';

@Component({
  selector: 'app-user-edit-form',
  templateUrl: './user-edit-form.component.html',
  styleUrls: ['./user-edit-form.component.scss'],
})
export class UserEditFormComponent implements OnInit {
  @ViewChild('container', { read: ViewContainerRef, static: true })
  private container!: ViewContainerRef;

  @ViewChild('passwordContainer', { read: ViewContainerRef, static: true })
  private passwordContainer!: ViewContainerRef;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private loggedInUserService: LoggedInUserService,
    private router: Router,
    private snackbar: MatSnackBar,
    private adapter: DateAdapter<any>,
    private dialog: MatDialog,
    private translate: TranslateService,
    private enumsService: EnumsService,
    private validatorService: ValidatorService,
    private loginService: LoginService,
    private route: ActivatedRoute,
    private shelterService: ShelterService
  ) { }

  personalDataForm!: FormGroup;
  passwordDataForm!: FormGroup;
  roleForm!: FormGroup;
  matcher = new CustomErrorStateMatcher();

  user!: User;

  hide: boolean = true;
  hideNew: boolean = true;
  hideConfirm: boolean = true;

  shelters: Shelter[] = []

  isAdminEditing: boolean = false;

  shelter!: Shelter

  shelterId!: number | undefined

  ngOnInit(): void {
    this.hide = true;
    this.hideNew = true;
    this.hideConfirm = true;
    let userId = 0;
    if (this.router.url.includes('user-form')) {
      this.route.params.subscribe(params => {
        this.isAdminEditing = true;
        userId = params['id'];
      });
    } else {
      userId = this.loggedInUserService.getLoggedInUser().id!;
    }

    this.adapter.setLocale(getLocale());

    this.getShelters()
    this.userService.getUserById(userId).subscribe({
      next: (user) => {
        this.user = user
        this.shelterId = user.shelterId
        this.roleForm = this.formBuilder.group({
          roleControl: [this.enumsService.getRoleFromOrdinal(this.user.role!), Validators.required],
          shelterControl: [user.shelterId]
        })

        if (!!user.shelterId) {
          this.shelterService.getShelterById(user.shelterId).subscribe((shelter) => {
            this.shelter = shelter
          })
        }

        this.personalDataForm = this.formBuilder.group({
          firstNameControl: [this.user.userDetails?.firstName, Validators.required],
          lastNameControl: [this.user.userDetails?.lastName, Validators.required],
          phoneNumberControl: [
            this.user.userDetails?.phoneNumber,
            [Validators.required, Validators.pattern(/^\d{9}$/)],
          ],
          cityControl: [this.user.userDetails?.city, Validators.required],
          postalCodeControl: [this.user.userDetails?.postalCode],
          streetControl: [this.user.userDetails?.street],
          streetNumberControl: [this.user.userDetails?.streetNumber],
          genderControl: [this.enumsService.getPersonGenderFromOrdinal(this.user.userDetails?.gender), Validators.required],
          birthDateControl: [this.user.userDetails?.birthDate, Validators.required],
        });

        this.passwordDataForm = this.formBuilder.group({
          oldPasswordControl: ['', Validators.required],
          newPasswordControl: [
            '',
            this.validatorService.getPasswordValidators(),
          ],
          confirmNewPasswordControl: [
            '',
            [Validators.required, this.validatorService.matchValidator('newPasswordControl')],
          ],
        });
      },
      error: (error) => {
      },
    });
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

    this.user.userDetails = personalData;



    this.user.role = this.roleForm.get('roleControl')?.value != null ?
      this.enumsService.getRoleOrdinal(this.roleForm.get('roleControl')?.value) : 2

    let id = this.roleForm.get('shelterControl')?.value
    this.user.shelterId = !!id ? parseInt(id) : undefined

    this.userService.updateUser(this.user).subscribe({
      next: (user) => {
        if (!this.isAdminEditing) {
          this.loggedInUserService.setLoggedInUser(this.user.id!);
          this.user = user
          this.router.navigate(['/data']);
        } else {
          this.router.navigate(['/users']);
        }

        this.translate
          .get('Snackbar.DataUpdated')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 2000 })
          );
      },
      error: (error) => {
        console.log(error)
      },
    });
  }

  isDifferentSignIn(): boolean {
    return this.loggedInUserService.getIsGoogleSignIn() || this.loggedInUserService.getIsFacebookSignIn();
  }

  onChangePassword() {
    let changePassword: ChangePassword = {
      oldPassword: this.passwordDataForm.get('oldPasswordControl')?.value,
      newPassword: this.passwordDataForm.get('newPasswordControl')?.value,
      repeatNewPassword: this.passwordDataForm.get('confirmNewPasswordControl')?.value
    };

    this.loginService.changePassword(changePassword).subscribe({
      next: () => {
        this.router.navigate(['/data']);
        this.translate
          .get('Snackbar.PasswordChanged')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 2000 })
          );
      },
      error: (error) => {
        console.log(error)
      },
    });
  }

  deleteAccount() {
    this.translate.get('Dialog.DeleteAccount').subscribe((dialog: string) => {
      const dialogRef = this.dialog.open(DialogBodyComponent, {
        width: '250px',
        data: {
          result: 0,
          message: dialog,
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result.data == 1) {
          this.userService.deleteUser(this.user.id!).subscribe(() => {
            this.loggedInUserService.logout();
            this.translate
              .get('Snackbar.AccountDeleted')
              .subscribe((message: string) =>
                this.snackbar.open(message, 'X', { duration: 2000 })
              );
          });
        }
      });
    });
  }

  isWorkerSelected() {
    return this.roleForm.get('roleControl')?.value != null &&
      this.roleForm.get('roleControl')?.value == Role.WORKER
  }

  getRole() {
    return this.enumsService.getRole();
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

  getShelter(shelterId: number | undefined) {
    if (!!shelterId) {
      this.shelterService.getShelterById(shelterId).subscribe((shelter) => {
        console.log(shelter)
        return shelter
      })
    }
  }
}
