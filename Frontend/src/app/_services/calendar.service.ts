import { Calendar } from './../_models/calendar';
import { BehaviorSubject, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CalendarService {
  private calendarSubject = new BehaviorSubject<Calendar>(null);
  calendar: Observable<Calendar>;
  dayNames = [
    'Monday',
    'Tuesday',
    'Wendesday',
    'Thursday',
    'Friday',
    'Saturday',
    'Sunday',
  ];

  constructor() {
    const today = new Date();

    this.setCalendar(today.getFullYear(), today.getMonth(), today.getDate());

    this.calendar = this.calendarSubject.asObservable();
  }

  changeDate(calendar: Calendar): void {
    this.calendarSubject.next(calendar);
  }

  public get calendarValue(): Calendar {
    // returning last value
    return this.calendarSubject.value;
  }

  setCalendar(year: number, month: number, day: number): void {
    const date = new Date(year, month, day);

    const monthName = date.toLocaleDateString('en-US', { month: 'long' });
    const dayNumber = date.getDay();

    const calendar = new Calendar(
      date.getDate(),
      this.dayNames[this.adjustDayNumber(dayNumber)],
      monthName,
      date.getMonth(),
      date.getFullYear()
    );

    this.changeDate(calendar);
  }

  private adjustDayNumber(dayNumber): number {
    let day = dayNumber;
    if (day === 0) {
      day = 7;
    }

    return day - 1;
  }

  changeDateEvent(event): void {
    const year = event.year;
    const month = event.month - 1;
    const day = event.day;

    this.setCalendar(year, month, day);
  }

  changeDateByOneDay(chevron: number): void {
    this.setCalendar(
      this.calendarValue.year,
      this.calendarValue.month,
      this.calendarValue.dayNumber + chevron
    );
  }
}
