import { CustomEncoder } from './../_helpers/custom-encoder';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { Jwt } from '../_models/jwt';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  // BehaviorSubject: special type of observable, which needs initial value, because it must always return a value on subscription
  // after subscription, returns the last value of the subject
  // getValue()to get last value in non-observable code
  private jwtSubject: BehaviorSubject<Jwt>;

  public jwt: Observable<Jwt>;

  private refreshTokenTimeout;

  constructor(private router: Router, private http: HttpClient) {
    this.jwtSubject = new BehaviorSubject<Jwt>( // necessity of initialization the BehaviourSubject
      // Parse to the JWT object
      JSON.parse(localStorage.getItem('jwt'))
    );
    // get an observable from BehaviourSubject
    // to prevent nexting values into to the subject
    // now can only listening, can't emit
    this.jwt = this.jwtSubject.asObservable();
  }

  public get jwtValue(): Jwt {
    // returning last value
    return this.jwtSubject.value;
  }

  signIn(username: string, password: string): Observable<Jwt> {
    return this.http
      .post<any>(`${environment.API_URL}/account/signin`, {
        username,
        password,
      })
      .pipe(
        map((jwt) => {
          // set the jwt in the local storage
          localStorage.setItem('jwt', JSON.stringify(jwt));
          // assign jwt to subject
          this.jwtSubject.next(jwt);
          this.startRefreshTokenTimer();
          return jwt;
        })
      );
  }

  signInWithFb(token: string): Observable<Jwt> {
    return this.http
      .post<any>(`${environment.API_URL}/facebook`, { accessToken: token })
      .pipe(
        map((jwt) => {
          localStorage.setItem('jwt', JSON.stringify(jwt));
          this.jwtSubject.next(jwt);
          this.startRefreshTokenTimer();
          return jwt;
        })
      );
  }

  signUp(username: string, email: string, password: string) {
    return this.http.post<any>(`${environment.API_URL}/account/signup`, {
      username,
      email,
      password,
    });
  }

  confirmEmail(userId: string, code: string) {
    // add params which are needed to confirm email
    let params = new HttpParams({ encoder: new CustomEncoder() });
    params = params.append('uid', userId);
    params = params.append('code', code);

    return this.http.get(`${environment.API_URL}/confirmemail`, {
      params,
    });
  }

  forgotPassword(email: string) {
    return this.http.post(`${environment.API_URL}/account/forgotpassword`, {
      email,
    });
  }

  logout(): void {
    // remove jwt from local storage
    localStorage.removeItem('jwt');

    // call revoke token to request the server to marked token as revoked
    this.revokeToken().subscribe(() => {
      this.jwtSubject.next(null);
      this.stopRefreshTokenTimer();
      this.router.navigate(['/sign/in']);
    });
  }

  refreshToken() {
    return this.http
      .post<any>(`${environment.API_URL}/tokens/use`, {
        refreshToken: this.jwtValue.refreshToken,
      })
      .pipe(
        map((jwt) => {
          this.jwtSubject.next(jwt);
          this.startRefreshTokenTimer();
          return jwt;
        })
      );
  }

  revokeToken() {
    return this.http.post<any>(`${environment.API_URL}/tokens/revoke`, {
      refreshToken: this.jwtValue.refreshToken,
    });
  }

  // count when token will expire and refresh its one minute before expiration
  private startRefreshTokenTimer() {
    const expires = new Date(this.jwtValue.expires);

    const timeout = expires.getTime() - Date.now() - 60 * 1000;

    this.refreshTokenTimeout = setTimeout(
      () => this.refreshToken().subscribe(),
      timeout
    );
  }

  // clear timer
  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
