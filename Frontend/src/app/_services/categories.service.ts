import { CategoryOfProduct } from './../_models/categoryOfProduct';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  constructor(private http: HttpClient) {}

  getCategories() {
    return this.http.get<CategoryOfProduct[]>(
      `${environment.API_URL}/categoriesofproduct`
    );
  }
}
