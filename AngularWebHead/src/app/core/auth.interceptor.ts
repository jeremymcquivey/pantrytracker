
import {tap} from 'rxjs/operators';
import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthService } from './auth.service';
import { Constants } from '../constants';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private _authService: AuthService, private _router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.url.startsWith(Constants.apiRoot)) {
      this._authService.getAccessToken().then(accessToken => {
        const headers = req.headers.set('Authorization', `Bearer ${accessToken}`);
        const authReq = req.clone({ headers });
        return next.handle(authReq).pipe(tap(() => {}, error => {
          var respError = error as HttpErrorResponse;
          if (respError && (respError.status === 401 || respError.status === 403)) {
            this._router.navigate(['/unauthorized']);
          }
        }));
      });
    } else {
      return next.handle(req);
    }
  }
}
