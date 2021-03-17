import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['../sign.component.css'],
})
export class SignUpComponent implements OnInit {
  constructor(private titleService: Title) {
    this.titleService.setTitle('Fitweb - Sign up');
  }

  ngOnInit(): void {}
}
