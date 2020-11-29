import { UserService } from './user.service';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  readonly BaseURI = 'https://localhost:44318/api';

  constructor(private formBuilder: FormBuilder, private userService: UserService, private http: HttpClient) { }

  formModel = this.formBuilder.group({
    UserId: null,
    Username: ['', [Validators.required, Validators.minLength(4)]],
    Email: ['', [Validators.required, Validators.email]]
  });

  passwordFormModel = this.formBuilder.group({
    UserId: null,
    Passwords: this.formBuilder.group({
      Password: ['', [Validators.required, Validators.pattern('^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{6,}$')]],
      ConfirmPassword: ['', Validators.required]
    }, {validator: this.userService.comparePasswords })
  });

  editProfile() {
    var body = {
      Username: this.formModel.value.Username,
      Email: this.formModel.value.Email
    };

    return this.http.post(this.BaseURI + '/account/editprofile', body);
  }
  editPassword() {
    var body = {
      NewPassword: this.passwordFormModel.value.Passwords.Password
    };

    return this.http.post(this.BaseURI + '/account/changepassword', body);
  }

  getCalories() {
    return this.http.get(this.BaseURI + '/account/calories');
  }


}
