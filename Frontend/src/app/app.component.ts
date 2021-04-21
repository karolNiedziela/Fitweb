import { Component, Host, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Fitweb';

  ngOnInit(): void {
    this.positionAlert();
  }

  positionAlert(): void {
    let nav = document.querySelector('nav');

    let navCoords = nav.getBoundingClientRect();

    let alert = document.querySelector<HTMLElement>('app-alert');
    alert.style.top = navCoords.top + nav.offsetHeight + 'px';
    alert.style.right = navCoords.left + 'px';
  }
}
