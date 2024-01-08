import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LoggedInUserService } from '../../../modules/authentication/login/services/logged-in-user/logged-in-user.service';
import { PersonalData } from '../../../modules/users/models/PersonalData';
import { Role, User } from '../../../modules/users/models/User';
import { Language } from '../../models/Language';
import { ChangeLanguageService } from '../../services/change-language/change-language.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopbarComponent implements OnInit {
  constructor(
    private loggedInService: LoggedInUserService,
    private router: Router,
    private translate: TranslateService,
    private translateLanguage: ChangeLanguageService,
    private enumsService: EnumsService
  ) {
    if (localStorage.getItem('language') === null) {
      translate.setDefaultLang('en');
      this.loggedInService.changeUsedLanguage('en');
    } else {
      translate.use(localStorage.getItem('language')!);
    }
  }

  toolbarConstants: string[] = [
    'Pets',
    'About adoption',
    'How to help'
  ];

  badgeNumber: number = 0;

  ngOnInit(): void {
    this.router.onSameUrlNavigation = 'reload';
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  isUserLoggedIn(): boolean {
    return this.loggedInService.getLoggedInUser() !== null;
  }

  isAdminLoggedIn(): boolean {
    return this.loggedInService.isAdminLoggedIn();
  }

  isWorkerLoggedIn(): boolean {
    return this.loggedInService.isWorkerLoggedIn();
  }

  isWorkerOnlyLoggedIn(): boolean {
    return this.loggedInService.isOnlyWorkerLoggedIn();
  }

  isOnlyUserLoggedIn() {
    return this.loggedInService.isOnlyUserLoggedIn();
  }

  getLoggedInUserData(): PersonalData {
    return this.loggedInService.getLoggedInUser().userDetails;
  }

  getLoggedInUser(): User {
    return this.loggedInService.getLoggedInUser();
  }

  logout() {
    this.loggedInService.logout();
  }

  displayPets() {
    this.router.navigate(['/pets']);
  }

  changeLanguage() {
    if (this.translate.currentLang === 'en') {
      this.translate.use('pl');
      this.loggedInService.changeUsedLanguage('pl');
      this.translateLanguage.changeLanguage(Language.PL).subscribe();
      window.location.reload();
    } else {
      this.translate.use('en');
      this.loggedInService.changeUsedLanguage('en');
      this.translateLanguage.changeLanguage(Language.EN).subscribe();
      window.location.reload();
    }
  }

  getLanguage(): string {
    if (this.translate.currentLang === undefined) {
      this.translate.use('en');
      localStorage.setItem('language', 'en');
    }
    if (this.translate.currentLang === 'en') {
      return 'PL';
    } else {
      return 'EN';
    }
  }
}
