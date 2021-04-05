import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  // CanActivate decides if route can be activated
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    // get jwt
    const jwt = this.authenticationService.jwtValue;
    // if jwt exists -> activate
    if (jwt) {
      return true;
    }

    // else -> redirect to sign in form

    this.router.navigate(['sign/in'], {
      queryParams: { returnUrl: state.url },
    });
    return false;
  }
}
