import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Day } from '../_models/day';

@Injectable({
  providedIn: 'root',
})
export class DayService {
  constructor(private http: HttpClient) {}

  get() {
    return this.http.get<Day[]>(`${environment.API_URL}/days`);
  }
}
