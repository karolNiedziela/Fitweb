import { UserService } from './../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  userLogged;

  constructor(private service: UserService, private router: Router) { }

  ngOnInit(): void {
    if(localStorage.getItem('token') != null) {
      this.userLogged = 1;
    }
    else {
      this.userLogged = 0;
    }

  }

  onLogout() {
    localStorage.removeItem('token');
    this.userLogged = 0;
    this.router.navigateByUrl('/home');
  }
}
