import { AlertService } from './../../_services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from './../../_services/authentication.service';
import {
  FormControl,
  FormGroup,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ComparePasswords } from '../../_validators/sign-up.validator';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['../sign.component.css'],
})
export class SignUpComponent implements OnInit {
  submitted = false;
  signUpForm: FormGroup;
  returnUrl: string;
  error = '';

  usernameRegex = /^[a-z0-9]{4, 40}$/;
  // 6 characters, 1 uppercase letter, 1 lowercase and 1 number
  passwordRegex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=.\-_*])([a-zA-Z0-9@#$%^&+=*.\-_]){6,20}/;

  constructor(
    private titleService: Title,
    private authenticationService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    private alert: AlertService
  ) {
    this.titleService.setTitle('Fitweb - Sign up');
  }

  ngOnInit(): void {
    this.signUpForm = new FormGroup({
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(40),
      ]),
      email: new FormControl('', [Validators.required, Validators.email]),
      passwords: new FormGroup(
        {
          password: new FormControl('', [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ]),
          confirmPassword: new FormControl('', [Validators.required]),
        },
        { validators: ComparePasswords }
      ),
    });

    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '';
  }

  get form() {
    return this.signUpForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.signUpForm.invalid) {
      return;
    }

    this.authenticationService
      .signUp(
        this.signUpForm.value.username,
        this.signUpForm.value.email,
        this.signUpForm.value.passwords.password
      )
      .subscribe(
        () => {
          this.alert.success(
            'Registration succesful. To get full access confirm email.',
            {
              keepAfterRouteChange: true,
            }
          );
          this.router.navigate([this.returnUrl]);
        },
        (error) => {
          console.log(error);
          this.error = error;
        }
      );
  }
}
