import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from './../_services/authentication.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css'],
})
export class EmailConfirmationComponent implements OnInit {
  showSuccess: boolean;
  showError: boolean;
  error: string;
  returnUrl: string;

  constructor(
    private authService: AuthenticationService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.confirmEmail();
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '';
  }

  private confirmEmail = () => {
    const userId = this.route.snapshot.queryParams['uid'];
    const code = this.route.snapshot.queryParams['code'];

    console.log(userId);
    console.log(code);

    this.authService.confirmEmail(userId, code).subscribe(
      (_) => {
        this.showSuccess = true;
      },
      (error) => {
        this.showError = true;
        this.error = error;
      }
    );
  };
}
