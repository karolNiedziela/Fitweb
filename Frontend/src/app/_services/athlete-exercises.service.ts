import { AthleteExercise } from './../_models/athleteExercise';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AthleteExercisesService {
  constructor(private http: HttpClient) {}

  getExercises(dayName): Observable<AthleteExercise> {
    return this.http.get<AthleteExercise>(
      `${environment.API_URL}/athletes/exercises?dayName=${dayName}`
    );
  }

  post(
    exerciseId: number,
    weight: number,
    numberOfSets: number,
    numberOfReps: number,
    dayName: string
  ) {
    return this.http.post<any>(`${environment.API_URL}/athletes/exercises`, {
      exerciseId,
      weight,
      numberOfSets,
      numberOfReps,
      dayName,
    });
  }

  put(
    exerciseId: number,
    weight: number,
    numberOfSets: number,
    numberOfReps: number,
    dayName: string
  ) {
    return this.http.put<any>(`${environment.API_URL}/athletes/exercises`, {
      exerciseId,
      weight,
      numberOfSets,
      numberOfReps,
      dayName,
    });
  }

  delete(exerciseId: number) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: {
        exerciseId,
      },
    };

    return this.http.request(
      'delete',
      `${environment.API_URL}/athletes/exercises`,
      options
    );
  }
}
