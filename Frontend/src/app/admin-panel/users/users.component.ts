import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

  term: any;
  p: 1;

  constructor(public userService: UserService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.userService.getAllUsers();
  }

  onDelete(id) {
    if (confirm('Are you sure to delete this user?')) {
      this.userService.deleteUser(id).subscribe(
        (res: any) => {
          this.userService.getAllUsers();
          this.toastr.warning('Deleted successfully.', 'Fitweb');

        },
        err => {
          console.log(err);
        }
      );
    }
  }

}
