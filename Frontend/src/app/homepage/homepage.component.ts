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

  ngOnInit(): void {
    let caption = document.querySelector<HTMLElement>('.caption');
    let homeLanding = document.querySelector('img');
    let navbar = document.querySelector<HTMLElement>('nav');

    homeLanding.onload = () => {
      caption.style.top = navbar.clientHeight + homeLanding.height * 0.5 + 'px';
    };
  }
}
