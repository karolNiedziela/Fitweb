import { Jwt } from './../_models/jwt';
import { AuthenticationService } from './../_services/authentication.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  currentUser: Jwt;
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.authenticationService.jwt.subscribe((x) => (this.currentUser = x));
  }

  redirect(): void {
    this.router.navigate(['sign', 'in']);
  }

  logout(): void {
    this.authenticationService.logout();
    this.router.navigate(['/sign/in']);
  }
}
