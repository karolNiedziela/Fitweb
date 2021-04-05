import { PaginationQuery } from './../_models/paginationQuery';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PageRequest } from '../_models/pageRequest';
import { Exercise } from '../_models/exercise';

@Injectable({
  providedIn: 'root',
})
export class ExerciseService {
  constructor(private http: HttpClient) {}

  getExercises(
    name: string,
    partOfBody: string,
    paginationQuery: PaginationQuery
  ): Observable<PageRequest<Exercise>> {
    return this.http.get<PageRequest<Exercise>>(
      `${environment.API_URL}/exercises?name=${name}&partOfBody=${partOfBody}&pageNumber=${paginationQuery.pageNumber}&pageSize=${paginationQuery.pageSize}`
    );
  }
}
