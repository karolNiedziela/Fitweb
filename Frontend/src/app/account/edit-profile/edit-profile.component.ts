import { ToastrService } from 'ngx-toastr';
import { UserService } from './../../shared/user.service';
import { AccountService } from './../../shared/account.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  user;

  constructor(public accountService: AccountService, public userService: UserService, private toastr: ToastrService,
              private router: Router) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.accountService.formModel.reset();
        this.user = res;
      },
      err => {
        console.log(err);
      }
    );
  }

  onSubmit() {
    this.accountService.editProfile().subscribe(
     (res: any) => {
      this.accountService.formModel.reset();
      this.toastr.success('Your profile changed succesfully', 'Fitweb');
      this.router.navigateByUrl('/account/account-detail');
     },
    (err: any) => {
        this.toastr.error(err.error.message, 'Edit failed');
     }
    );
  }

}
