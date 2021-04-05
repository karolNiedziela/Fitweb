import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AlertService } from '../_services/alert.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css'],
})
export class HomepageComponent implements OnInit {
  constructor(private titleService: Title) {
    this.titleService.setTitle('Fitweb');
  }

  ngOnInit(): void {}
}
