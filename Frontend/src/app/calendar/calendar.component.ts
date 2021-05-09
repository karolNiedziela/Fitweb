import { CalendarService } from './../_services/calendar.service';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  NgbDateStruct,
  NgbCalendar,
  NgbDatepicker,
} from '@ng-bootstrap/ng-bootstrap';
import { Calendar } from '../_models/calendar';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css'],
})
export class CalendarComponent implements OnInit {
  calendar: Calendar;
  model: NgbDateStruct;
  datePicker: NgbDatepicker;
  pickedDate: { year: number; month: number; day: number };

  constructor(public calendarService: CalendarService) {}

  ngOnInit(): void {
    this.calendarService.calendar.subscribe((calendar) => {
      this.calendar = calendar;
    });
  }

  // pick date in datepicker
  changeDate(event) {
    this.pickedDate = event;

    this.calendarService.setCalendar(
      this.pickedDate.year,
      this.pickedDate.month - 1,
      this.pickedDate.day
    );
  }

  // Decrease or increase date on click chevron
  changeDateByOneDay(number) {
    this.calendarService.setCalendar(
      this.calendar.year,
      this.calendar.month,
      this.calendar.dayNumber + number
    );
  }

  show() {
    let calendar = document.querySelector<HTMLElement>('.datepicker');

    calendar.hidden = !calendar.hidden;
  }
}
