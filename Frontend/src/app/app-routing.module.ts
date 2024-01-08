import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllAppointmentsPageComponent } from './modules/appointments/all-appointments-page/all-appointments-page.component';
import { UserAppointmentsPageComponent } from './modules/appointments/user-appointments-page/user-appointments-page.component';
import { ConfirmRegistrationComponent } from './modules/authentication/registation/confirm-registration/confirm-registration.component';
import { LoginFormComponent } from './modules/authentication/login/login-form/login-form.component';
import { PetFormComponent } from './modules/pets/pet-form/pet-form.component';
import { PetPageElementComponent } from './modules/pets/pet-page-element/pet-page-element.component';
import { PetPageComponent } from './modules/pets/pet-page/pet-page.component';
import { RestorePasswordEmailComponent } from './modules/authentication/restore-password/restore-password-email/restore-password-email.component';
import { RestorePasswordComponent } from './modules/authentication/restore-password/restore-password/restore-password.component';
import { UserEditFormComponent } from './modules/users/user-edit-form/user-edit-form.component';
import { UserFormComponent } from './modules/authentication/registation/register-form/register-form.component';
import { UsersPageComponent } from './modules/users/users-page/users-page.component';
import { AdminGuard } from './core/guards/admin/admin.guard';
import { LoggedInGuard } from './core/guards/login/logged-in.guard';
import { RegisterGuard } from './core/guards/register/register.guard';
import { UserGuard } from './core/guards/user/user.guard';
import { ShelterFormComponent } from './modules/shelter/shelter-form/shelter-form.component';
import { ShelterPageComponent } from './modules/shelter/shelter-page/shelter-page.component';
import { WorkerGuard } from './core/guards/worker/worker.guard';
import { ShelterPetsPageComponent } from './modules/pets/shelter-pets-page/shelter-pets-page/shelter-pets-page.component';


const routes: Routes = [
  { path: '', component: PetPageComponent },
  { path: 'pets', component: PetPageComponent },
  { path: 'pets/:id', component: PetPageElementComponent },
  { path: 'pet-form', component: PetFormComponent, canActivate: [ WorkerGuard] },
  { path: 'pet-form/:id', component: PetFormComponent, canActivate: [ WorkerGuard] },
  { path: 'shelter-form', component: ShelterFormComponent, canActivate: [ WorkerGuard] },
  { path: 'shelter-form/:id', component: ShelterFormComponent, canActivate: [ WorkerGuard] },
  { path: 'our-shelter', component: ShelterPetsPageComponent, canActivate: [ WorkerGuard] },
  { path: 'shelters', component: ShelterPageComponent, canActivate: [AdminGuard]},
  { path: 'login', component: LoginFormComponent, canActivate: [LoggedInGuard] },
  { path: 'register', component: UserFormComponent, canActivate: [RegisterGuard] },
  { path: 'data', component: UserEditFormComponent, canActivate: [UserGuard] },
  { path: 'user-form/:id', component: UserEditFormComponent, canActivate: [AdminGuard] },
  { path: 'my-appointments', component: UserAppointmentsPageComponent, canActivate: [UserGuard] },
  { path: 'all-appointments', component: AllAppointmentsPageComponent, canActivate: [ WorkerGuard] },
  { path: 'verify-registration-success', component: ConfirmRegistrationComponent },
  { path: 'reset-password', component: RestorePasswordComponent, canActivate: [LoggedInGuard] },
  { path: 'restore-password-email', component: RestorePasswordEmailComponent, canActivate: [LoggedInGuard] },
  { path: 'users', component: UsersPageComponent, canActivate: [AdminGuard]}
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
