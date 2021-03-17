import { AuthenticationService } from './../_services/authentication.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // get jwt
    const jwt = this.authenticationService.jwtValue;
    // check if user is already logged in
    const isLoggedIn = jwt && jwt.accessToken;
    // check if url is correct
    const isApiUrl = request.url.startsWith(environment.API_URL);
    // if user is logged in and api url is correct
    // -> clone the request before sending to the server and add authorization header
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${jwt.accessToken}`,
        },
      });
    }

    // pass the modified request object
    return next.handle(request);
  }
}
