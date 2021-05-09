import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  put(productId, athleteProductId, weight) {
    return this.http.put<any>(`${environment.API_URL}/athletes/products`, {
      productId,
      athleteProductId,
      weight,
    });
  }

  delete(productId, athleteProductId) {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: {
        productId,
        athleteProductId,
      },
    };
    return this.http.request<any>(
      'delete',
      `${environment.API_URL}/athletes/products`,
      options
    );
  }
}
