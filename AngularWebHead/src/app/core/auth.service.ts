import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserManager, User, WebStorageStateStore } from 'oidc-client';
import { Constants } from '../constants';
import { Subject, from, Observable } from 'rxjs';
import { SecurityContext } from '../model/authmodel';

@Injectable()
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();
  private _authContextChangedSubject = new Subject<SecurityContext>();
  authContext: SecurityContext;

  loginChanged = this._loginChangedSubject.asObservable();
  authContextChanged = this._authContextChangedSubject.asObservable();

  constructor(private _httpClient: HttpClient) {
    var config = {
      authority: Constants.stsAuthority,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}signin-callback`,
      scope: 'openid pantrytrackers-api profile',
      response_type: 'id_token token',
      post_logout_redirect_uri: `${Constants.clientRoot}signout-callback`,
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      automaticSilentRenew: true,
      silent_redirect_uri: `${Constants.clientRoot}assets/silent-callback.html`
    };
    this._userManager = new UserManager(config);
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
    this._userManager.events.addUserLoaded(user => {
      if (this._user !== user) {
        this._user = user;
        this.loadSecurityContext('addUserLoaded');
        this._loginChangedSubject.next(!!user && !user.expired);
      }
    });
  }
  
  login(): Promise<any> {
    return this._userManager.signinRedirect();
  }

  logout(): Promise<any> {
    this._user = null;
    return this._userManager.signoutRedirect();
  }

  isLoggedIn(): Promise<boolean> {
    return this._userManager.getUser().then(user => {
      const userCurrent = !!user && user.access_token && !user.expired;
      if (this._user !== user) {
        this._loginChangedSubject.next(userCurrent);
      }
      if (userCurrent && !this.authContext) {
        this.loadSecurityContext('isLoggedIn');
      }
      this._user = user;
      return !!user && !user.expired;
    });
  }

  completeLogin(): Promise<any> {
    return this._userManager.signinRedirectCallback().then(user => {
      this._user = user;
      this._loginChangedSubject.next(!!user && !user.expired);
      return user;
    });
  }

  completeLogout(): Promise<any> {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  }

  getAccessToken(): Promise<string> {
    return this._userManager.getUser().then(user => {
      if (!!user && !user.expired) {
        return user.access_token;
      }
      else {
        return null;
      }
    });
  }

  signoutRedirectCallback(): Promise<any> {
    return this._userManager.signoutRedirectCallback();
  }

  loadSecurityContext(source: string) {
    this.fetchSecurityContext().subscribe(context => {
        this.authContext = new SecurityContext();
        this.authContext.roles = context.roles;
        this.authContext.userProfile = context.userProfile;
        this._authContextChangedSubject.next(this.authContext);
    }, error => console.error(error));
  }

  fetchSecurityContext(): Observable<SecurityContext> {
    return from (this.getAccessToken().then(accessToken => {
      var headers = new HttpHeaders().set('Authorization', `Bearer ${accessToken}`);
      return this._httpClient.get<SecurityContext>(`${Constants.recipeApi}v1/User/AuthContext`, { headers: headers }).toPromise();
    }));
  }
}
