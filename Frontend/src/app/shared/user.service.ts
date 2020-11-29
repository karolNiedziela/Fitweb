import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly BaseURI = 'https://localhost:44318/api';

  constructor(private formBuilder: FormBuilder, private httpClient: HttpClient) { }

  formModel = this.formBuilder.group({
    Username: ['', Validators.required],
    Email: ['', [Validators.required, Validators.email]],
    Passwords: this.formBuilder.group({
      Password: new FormControl('', [Validators.required]),
      ConfirmPassword: ['', Validators.required]
    }, 
    {validator: this.comparePasswords }),
    Role: 'User'
  });

  user: User =
  {
    products: null,
    exercises: null,
    id: null,
    username: '',
    email: '',
    role: '',
    createdAt: null
  };

  users: User[];

  comparePasswords(formGroup: FormGroup) {
    let confirmPasswordControl = formGroup.get('ConfirmPassword');

    if (confirmPasswordControl.errors == null || 'passwordMismatch' in confirmPasswordControl.errors) {
      if (formGroup.get('Password').value != confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({ passwordMismatch: true });
      }
      else {
        confirmPasswordControl.setErrors(null);
      }
    }
  }

  register() {
    var body = {
      Username: this.formModel.value.Username,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Passwords.Password,
      Role: 'User'
    };

    return this.httpClient.post(this.BaseURI + '/users/register', body);
  }

  login(formData) {
    return this.httpClient.post(this.BaseURI + '/users/login', formData);
  }

  getUserAccount() {
    return this.httpClient.get(this.BaseURI + '/account');
  }

  getAllUsers() {
    return this.httpClient.get(this.BaseURI + '/users')
    .toPromise()
    .then(res => this.users = res as User[]);
  }

  deleteUser(id) {
    return this.httpClient.delete(this.BaseURI + '/users/' + id, id);
  }


  roleMatch(allowedRoles): boolean {
    var isMatch = false;
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    var userRole = payLoad['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    allowedRoles.forEach(element => {
      if (userRole == element) {
        isMatch = true;
        return false;
      }
    });

    return isMatch;
  }
}
