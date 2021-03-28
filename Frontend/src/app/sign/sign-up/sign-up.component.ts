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

  constructor(
    private titleService: Title,
    private authenticationService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.titleService.setTitle('Fitweb - Sign up');
  }

  ngOnInit(): void {
    this.signUpForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      passwords: new FormGroup(
        {
          password: new FormControl('', [Validators.required]),
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

    console.log(this.signUpForm.value.userName);
    console.log(this.signUpForm.value.email);
    console.log(this.signUpForm.value.passwords.password);
    this.authenticationService
      .signUp(
        this.signUpForm.value.userName,
        this.signUpForm.value.email,
        this.signUpForm.value.passwords.password
      )
      .subscribe(
        () => {
          this.router.navigate([this.returnUrl]);
        },
        (error) => {
          console.log(error);
          this.error = error;
        }
      );
  }
}
