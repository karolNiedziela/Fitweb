import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FacebookLoginProvider,
  SocialAuthService,
  SocialUser,
} from 'angularx-social-login';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-user',
  templateUrl: './sign.component.html',
  styleUrls: ['./sign.component.css'],
})
export class SignComponent implements OnInit {
  platform: string;
  loading = false;
  returnUrl: string;

  constructor(
    private socialAuthService: SocialAuthService,
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '';
  }

  signInWithFacebook() {
    this.loading = true;
    this.socialAuthService
      .signIn(FacebookLoginProvider.PROVIDER_ID)
      .then((response) => {
        console.log(this.platform + 'logged in user data is= ', response);

        const token = response.authToken;

        this.authService.signInWithFb(token).subscribe(
          (data) => {
            this.router.navigate([this.returnUrl]);
          },
          (error) => {
            console.log(error);
            this.loading = false;
          }
        );
      });
  }
}
