import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from "@angular/flex-layout";

import {TranslateLoader, TranslateModule} from "@ngx-translate/core";
import {TranslateHttpLoader} from "@ngx-translate/http-loader";

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TopbarComponent } from './core/components/topbar/topbar.component';
import { HttpInterceptorService } from './core/http-interceptor/http-interceptor.service';
import { PetPageComponent } from './modules/pets/pet-page/pet-page.component';
import { PetFormComponent } from './modules/pets/pet-form/pet-form.component';
import { LoginFormComponent } from './modules/authentication/login/login-form/login-form.component';
import { UserFormComponent } from './modules/authentication/registation/register-form/register-form.component';
import { CarouselComponent } from './shared/components/carousel/carousel.component';
import { DialogBodyComponent } from './shared/components/dialog-body/dialog-body.component';
import { PetPageElementComponent } from './modules/pets/pet-page-element/pet-page-element.component';


import {
  RECAPTCHA_SETTINGS,
  RecaptchaFormsModule,
  RecaptchaModule,
  RecaptchaSettings,
} from 'ng-recaptcha';
import { environment } from 'src/environments/environment';
import { PetFiltersComponent } from './modules/pets/pet-filters/pet-filters.component';
import { AppointmentSchedulerComponent } from './modules/appointments/appointment-scheduler/appointment-scheduler.component';
import { UserEditFormComponent } from './modules/users/user-edit-form/user-edit-form.component';
import { LogoutDialogComponent } from './modules/authentication/login/logout-dialog/logout-dialog.component';
import { UserAppointmentsPageComponent } from './modules/appointments/user-appointments-page/user-appointments-page.component';
import { ConfirmRegistrationComponent} from './modules/authentication/registation/confirm-registration/confirm-registration.component';
import { AllAppointmentsPageComponent } from './modules/appointments/all-appointments-page/all-appointments-page.component';
import { RestorePasswordComponent } from './modules/authentication/restore-password/restore-password/restore-password.component';
import { RestorePasswordEmailComponent } from './modules/authentication/restore-password/restore-password-email/restore-password-email.component';
import { UsersPageComponent } from './modules/users/users-page/users-page.component';
import { MapComponent } from './modules/map/map.component';
import { MaterialModule } from './material.module';
import { ShelterPageComponent } from './modules/shelter/shelter-page/shelter-page.component';
import { ShelterFormComponent } from './modules/shelter/shelter-form/shelter-form.component';
import { ShelterPetsPageComponent } from './modules/pets/shelter-pets-page/shelter-pets-page/shelter-pets-page.component';


export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}


@NgModule({
  declarations: [
    AppComponent,
    TopbarComponent,
    PetPageComponent,
    PetFormComponent,
    LoginFormComponent,
    UserFormComponent,
    CarouselComponent,
    DialogBodyComponent,
    PetPageElementComponent,
    PetFiltersComponent,
    AppointmentSchedulerComponent,
    UserEditFormComponent,
    LogoutDialogComponent,
    UserAppointmentsPageComponent,
    ConfirmRegistrationComponent,
    AllAppointmentsPageComponent,
    RestorePasswordComponent,
    RestorePasswordEmailComponent,
    UsersPageComponent,
    MapComponent,
    ShelterPageComponent,
    ShelterFormComponent,
    ShelterPetsPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'en',
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true,
    },
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.siteKey,
      } as RecaptchaSettings,
    },

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
