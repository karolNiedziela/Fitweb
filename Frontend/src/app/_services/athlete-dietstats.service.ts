import { AthleteDietStats } from './../_models/athleteDietStats';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DietStat } from '../_models/dietStat';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AthleteDietstatsService {
  constructor(private http: HttpClient) {}

  getDietStats(date: string): Observable<AthleteDietStats> {
    return this.http.get<AthleteDietStats>(
      `${environment.API_URL}/athletes/dietstats?date=${date}`
    );
  }
}
