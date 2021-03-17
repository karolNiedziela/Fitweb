import { AfterViewInit, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css'],
})
export class FooterComponent implements OnInit, AfterViewInit {
  constructor() {}

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    const contacts = document.querySelectorAll('.contact');

    contacts.forEach((contact) => {
      contact.addEventListener('mouseover', () => {
        moveIconUp(contact);
      });

      contact.addEventListener('mouseout', () => {
        undoIconUp(contact);
      });
    });
  }
}

function moveIconUp(contact): void {
  const i = Array.from(
    contact.getElementsByTagName('I') as HTMLCollectionOf<HTMLElement>
  )[0];
  i.classList.add('contact-i');
}

function undoIconUp(contact): void {
  const i = Array.from(
    contact.getElementsByTagName('I') as HTMLCollectionOf<HTMLElement>
  )[0];

  i.classList.remove('contact-i');
}
