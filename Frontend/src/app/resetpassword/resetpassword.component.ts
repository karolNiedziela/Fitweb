import { AlertService } from './../_services/alert.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ResetPasswordService } from './../_services/reset-password.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ComparePasswords } from '../_validators/sign-up.validator';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css'],
})
export class ResetPasswordComponent implements OnInit {
  showSuccess: boolean;
  showError: boolean;
  error: string;
  returnUrl: string;
  resetPasswordForm: FormGroup;
  submitted = false;
  private userId: string;
  private code: string;

  constructor(
    private route: ActivatedRoute,
    private resetPasswordService: ResetPasswordService,
    private alert: AlertService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.resetPassword();

    this.resetPasswordForm = new FormGroup({
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
    return this.resetPasswordForm.controls;
  }

  private resetPassword = () => {
    this.userId = this.route.snapshot.queryParams['uid'];
    this.code = this.route.snapshot.queryParams['code'];

    console.log(this.userId);
    console.log(this.code);

    this.resetPasswordService
      .getResetPassword(this.userId, this.code)
      .subscribe(
        (_) => {
          this.showSuccess = true;
        },
        (error) => {
          this.showError = true;
          this.error = error;
        }
      );
  };

  onSubmit(): void {
    this.submitted = true;

    if (this.resetPasswordForm.invalid) {
      return;
    }

    let password = this.resetPasswordForm.value.passwords.password;
    this.resetPasswordService.post(this.userId, this.code, password).subscribe(
      () => {
        this.alert.success(
          'Password was changed successfully. Now you can sign in.',
          {
            keepAfterRouteChange: true,
          }
        );
        this.returnUrl = '/sign/in';
        this.router.navigate([this.returnUrl]);
      },
      (error) => {
        this.error = error;
      }
    );
  }
}
