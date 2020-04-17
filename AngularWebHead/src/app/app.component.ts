import { Component, OnInit, ViewChildren, ElementRef, HostListener } from '@angular/core';
import { UserProfile } from './model/user-profile';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from './core/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['app.component.css']
})
export class AppComponent implements OnInit {
  userProfile: UserProfile;
  loginBusy = false;
  isLoggedIn = false;

  @ViewChildren("menuLink")
  menuLinks: ElementRef[] = [];

  // Before using @HostListener anywhere else, read this SO post:
  // https://stackoverflow.com/questions/40107008/detect-click-outside-angular-component
  @HostListener('document:click', ['$event'])
  documentClick(evt: any): void {
    if(evt.srcElement.type != 'checkbox' || evt.srcElement.name != 'menuLink') {
      this.menuLinks.forEach(cb => {
        cb.nativeElement.checked = false;
      });
    }
  }

  userName(): string {
    if(this._authService.authContext && this._authService.authContext.userProfile) {
      return this._authService.authContext.userProfile.firstName;
    }
    return '';
  }

  constructor(
    public dialog: MatDialog,
    private _authService: AuthService,
    private _router: Router
  ) {
    this._authService.loginChanged.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    })
  }

  ngOnInit() {
    this._authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  menuClickHandler(evt) {
    this.menuLinks.filter(cb => cb.nativeElement.checked && evt.srcElement.id !== cb.nativeElement.id)
                  .forEach(cb => cb.nativeElement.checked = false);
  }

  login() {
    this.loginBusy = true;
    this._authService.login();
  }

  logout() {
    this.loginBusy = true;
    this._authService.logout();
  }

  adminNavigation() {
    this._router.navigate(['admin']);
  }

  isAdmin() {
    return this._authService.authContext && this._authService.authContext.userIsAdmin();
  }
}