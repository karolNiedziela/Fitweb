import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { Jwt } from '../_models/jwt';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  // BehaviorSubject: special type of observable, which needs initial value, because it must always return a value on subscription
  // after subscription, returns the last value of the subject
  // getValue()to get last value in non-observable code
  private jwtSubject: BehaviorSubject<Jwt>;

  public jwt: Observable<Jwt>;

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
          return jwt;
        })
      );
  }

  logout(): void {
    // remove jwt from local storage
    localStorage.removeItem('jwt');

    // assign null value to subject
    this.jwtSubject.next(null);

    this.router.navigate(['/sign/in']);
  }
}
