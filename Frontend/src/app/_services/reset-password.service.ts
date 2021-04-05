import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CustomEncoder } from '../_helpers/custom-encoder';

@Injectable({
  providedIn: 'root',
})
export class ResetPasswordService {
  constructor(private http: HttpClient) {}

  getResetPassword(userId: string, code: string) {
    // add params which are needed to confirm email
    let params = new HttpParams({ encoder: new CustomEncoder() });
    params = params.append('uid', userId);
    params = params.append('code', code);

    return this.http.get(`${environment.API_URL}/resetpassword`, {
      params,
    });
  }

  post(userId: string, code: string, newPassword: string) {
    return this.http.post(`${environment.API_URL}/resetpassword`, {
      userId,
      code,
      newPassword,
    });
  }
}
