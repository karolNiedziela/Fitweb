import { AuthenticationService } from './authentication.service';
import { User } from './../_models/user';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  getMe() {
    return this.http.get<User>(`${environment.API_URL}/account/me`);
  }
}
