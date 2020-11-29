import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/shared/account.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-edit-password',
  templateUrl: './edit-password.component.html',
  styleUrls: ['./edit-password.component.css']
})
export class EditPasswordComponent implements OnInit {

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
    this.accountService.editPassword().subscribe(
     (res: any) => {
      this.accountService.formModel.reset();
      this.toastr.success('Your password changed succesfully', 'Fitweb');
      this.router.navigateByUrl('/account/account-detail');
     },
    (err: any) => {
        this.toastr.error(err.error.message, 'Edit failed');
     }
    );
  }


}
