import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Role } from 'src/app/core/extension/role.enum';
import { IUser } from 'src/app/core/interfaces/user.interface';
import { AuthenticationService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  currentUser: IUser;
  roles = Role;
  public currentUserSubject: BehaviorSubject<string>;
  public currentSubject: Observable<string>;
  constructor(private authService: AuthenticationService, private router: Router,
              @Inject(DOCUMENT) private document: Document,
              private translate: TranslateService) {
    this.currentUserSubject = new BehaviorSubject<string>(localStorage.getItem('localization'));
    this.currentSubject = this.currentUserSubject.asObservable();
    authService.currentUser.subscribe( x => this.currentUser = x);
    this.setTheme(this.isDarkTheme());
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  ngOnInit(): void {
    console.log(this.currentUser)
    if (!this.currentUserValue) {
      this.localizationEN();
    }
    this.translate.use(this.currentUserValue);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  localizationUA(): void {
    localStorage.setItem('localization', 'ua');
    this.currentUserSubject.next('ua');
    this.translate.use(this.currentUserValue);
  }

  localizationEN(): void {
    localStorage.setItem('localization', 'en');
    this.currentUserSubject.next('en');
    this.translate.use(this.currentUserValue);
  }

  getPatientOrganRoute(): string[] {
   return ['/artificial-organs/patient-organs/' + this.currentUser?.id];
  }

  setNewTheme(isDarkTheme: boolean) {
    localStorage.setItem('isDarkTheme', JSON.stringify(isDarkTheme));
    this.setTheme(isDarkTheme);
  }

  setTheme(isDarkTheme: boolean): void {
    const classList = this.document.body.classList;
    if (isDarkTheme) {
      classList.add('theme-alternate');
    } else {
      classList.remove('theme-alternate');
    }
  }

  isDarkTheme(): boolean {
    let isDarkTheme = JSON.parse(localStorage.getItem('isDarkTheme') || 'false');
    if (typeof isDarkTheme !== 'boolean') {
      isDarkTheme = false;
    }
    return isDarkTheme;
  }

  isDarkThemeAsync(): Observable<boolean> {
    let isDarkTheme = JSON.parse(localStorage.getItem('isDarkTheme') || 'false');
    if (typeof isDarkTheme !== 'boolean') {
      isDarkTheme = false;
    }
    return of(isDarkTheme);
  }
}
