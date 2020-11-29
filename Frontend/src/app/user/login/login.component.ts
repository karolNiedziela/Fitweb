import { HeaderComponent } from './../../header/header.component';
import { UserService } from './../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../user.component.css']
})
export class LoginComponent implements OnInit {

  formModel = {
    Username: '',
    Password: '',
  };
  constructor(private service: UserService, private router: Router, private toastr: ToastrService, private header: HeaderComponent) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/home');
    }
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.header.userLogged = 1;
        location.reload();
        this.router.navigateByUrl('/home');
      },
      err => {
        if (err.status == 400) {
          this.toastr.error('Incorrect credentials.', 'Authentication failed.');
        }
        else {
          console.log(err);
        }
      }
    );

  }

}
