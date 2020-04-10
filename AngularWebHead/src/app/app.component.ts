import { Component, OnInit } from '@angular/core';
import { AccountService } from './core/account.service';
import { UserProfile } from './model/user-profile';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from './core/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  userProfile: UserProfile;
  firstLogin = false;
  loginBusy = false;
  isLoggedIn = false;

  constructor(
    public dialog: MatDialog,
    private _authService: AuthService,
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

  isAdmin() {
    return this._authService.authContext && this._authService.authContext.userIsAdmin();
  }
}