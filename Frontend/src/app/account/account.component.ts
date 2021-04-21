import { Router } from '@angular/router';
import { Component, HostListener, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
export class AccountComponent implements OnInit, AfterViewInit {
  constructor() {}

  @HostListener('window:resize', [])
  private onResize() {
    this.detectScreenSize();
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.detectScreenSize();
  }

  private detectScreenSize(): void {
    const width974 = window.matchMedia('(max-width:974px)');
    const dropdown = document.querySelector<HTMLElement>(
      '.dropdown-sidebar-panel'
    );
    const img = document.querySelector('img');
    const menuList = document.querySelector<HTMLElement>('.menu-list');
    if (width974.matches) {
      dropdown.hidden = false;
      img.hidden = true;
      menuList.hidden = true;
    } else {
      dropdown.hidden = true;
      img.hidden = false;
      menuList.hidden = false;
    }
  }
}
