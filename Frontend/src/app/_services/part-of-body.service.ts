import { PartOfBody } from './../_models/partOfBody';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PartOfBodyService {
  constructor(private http: HttpClient) {}

  getParts() {
    return this.http.get<PartOfBody[]>(`${environment.API_URL}/partsofbody`);
  }
}
