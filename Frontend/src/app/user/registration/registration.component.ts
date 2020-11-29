import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['../user.component.css'],
})
export class RegistrationComponent implements OnInit {
  constructor(public service: UserService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.service.formModel.reset();
  }

  onSubmit() {
    this.service.register().subscribe(
     (res: any) => {
      this.service.formModel.reset();
      this.toastr.success('New user created!', 'Registration succesful.');
     },
    (err: any) => {
        this.toastr.error(err.error.message, 'Registration failed');
     }
    );
  }
}
