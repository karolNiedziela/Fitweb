export class Calendar {
  dayNumber: number;
  dayName: string;
  monthName: string;
  month: number;
  year: number;

  constructor(
    dayNumber: number,
    dayName: string,
    monthName: string,
    month: number,
    year: number
  ) {
    this.dayNumber = dayNumber;
    this.dayName = dayName;
    this.monthName = monthName;
    this.month = month;
    this.year = year;
  }
}
