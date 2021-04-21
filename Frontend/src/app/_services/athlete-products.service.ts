import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AthleteProduct } from '../_models/athleteProduct';

@Injectable({
  providedIn: 'root',
})
export class AthleteProductsService {
  constructor(private http: HttpClient) {}

  getProducts(date): Observable<AthleteProduct> {
    return this.http.get<AthleteProduct>(
      `${environment.API_URL}/athletes/products?date=${date}`
    );
  }

  post(productId, weight) {
    return this.http.post<any>(`${environment.API_URL}/athletes/products`, {
      productId,
      weight,
    });
  }
}
