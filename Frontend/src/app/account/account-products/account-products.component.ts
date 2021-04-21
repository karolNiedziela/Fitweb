import { AthleteDietStats } from './../../_models/athleteDietStats';
import { AthleteDietstatsService } from './../../_services/athlete-dietstats.service';
import { CalendarService } from './../../_services/calendar.service';
import { AthleteProduct } from './../../_models/athleteProduct';
import { AthleteProductsService } from './../../_services/athlete-products.service';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Calendar } from 'src/app/_models/calendar';
import { DietStat } from 'src/app/_models/dietStat';

@Component({
  selector: 'app-account-products',
  templateUrl: './account-products.component.html',
  styleUrls: ['./account-products.component.css'],
})
export class AccountProductsComponent implements OnInit {
  athleteProducts: AthleteProduct;
  calendar: Calendar;
  athleteDietStats: AthleteDietStats;

  constructor(
    private athleteProductsService: AthleteProductsService,
    private calendarService: CalendarService,
    private athleteDietStatsService: AthleteDietstatsService
  ) {}

  ngOnInit(): void {
    this.athleteProductsService.getProducts(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteProducts = response;
      },
      (error) => {
        console.log(error);
      }
    );

    this.calendarService.calendar.subscribe(
      (calendar) => (this.calendar = calendar)
    );

    this.athleteDietStatsService.getDietStats(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteDietStats = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getDateFormat(): string {
    let year = this.calendarService.calendarValue.year.toString();
    let month = (this.calendarService.calendarValue.month + 1).toString();
    if (+month < 10) {
      month = 0 + month;
    }
    let day = this.calendarService.calendarValue.dayNumber.toString();
    if (+day < 10) {
      day = 0 + day;
    }
    return `${year}-${month}-${day}`;
  }

  get() {
    console.log(this.getDateFormat());
    this.athleteProductsService.getProducts(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteProducts = response;
      },
      (error) => {
        console.log(error);
      }
    );

    this.athleteDietStatsService.getDietStats(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteDietStats = response;
        console.log(response);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
