import { Component, OnInit } from '@angular/core';
import { AccountService } from './core/account.service';
import { UserProfile } from './model/user-profile';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from './core/auth.service';
import { Router, RouterOutlet, RouterLink } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  userProfile: UserProfile;
  firstLogin = false;
  loginBusy = false;
  isLoggedIn = false;
  settingsExpanded = false;

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
    })
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
    this.settingsExpanded = false;
  }

  isAdmin() {
    return this._authService.authContext && this._authService.authContext.userIsAdmin();
  }
}