import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from './../_services/alert.service';
import { AuthenticationService } from './../_services/authentication.service';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  submitted = false;
  returnUrl: string;

  constructor(
    private authService: AuthenticationService,
    private alert: AlertService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
    });

    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '';
  }

  get form() {
    return this.forgotPasswordForm.controls;
  }

  onSubmit() {
    this.submitted = true;

    if (this.forgotPasswordForm.invalid) {
      return;
    }

    let email = this.forgotPasswordForm.value.email;
    console.log(email);
    this.authService.forgotPassword(email).subscribe(
      () => {
        this.alert.success(
          `Your forgot password email was sent to ${email}. Check your email.`,
          {
            keepAfterRouteChange: true,
          }
        );
        this.router.navigate([this.returnUrl]);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
