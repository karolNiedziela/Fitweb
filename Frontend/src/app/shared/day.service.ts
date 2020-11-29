import { Day } from './../models/day.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ThrowStmt } from '@angular/compiler';

@Injectable({
    providedIn: 'root'
  })

export class DayService {

    readonly BaseURI = 'https://localhost:44318/api';

    constructor(private http: HttpClient) { }

    day: Day = {
      id: null,
      name: ''
    };

    days: Day[] = [];

    getAllDays() {
      return this.http.get(this.BaseURI + '/days')
      .toPromise()
      .then((res: any) => this.days = res as Day[]);
    }
}
