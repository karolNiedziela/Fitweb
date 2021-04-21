import { AuthenticationService } from './../../_services/authentication.service';
import { AccountService } from './../../_services/account.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['../account.component.css', './account-overview.component.css'],
})
export class AccountOverviewComponent implements OnInit {
  user: User;
  constructor(
    private accountService: AccountService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.accountService.getMe().subscribe(
      (user) => {
        this.user = user;
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
