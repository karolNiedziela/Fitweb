import { PaginationQuery } from './../_models/paginationQuery';
import { PageRequest } from './../_models/pageRequest';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../_models/product';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  getProducts(
    name: string,
    category: string,
    paginationQuery: PaginationQuery
  ): Observable<PageRequest<Product>> {
    return this.http.get<PageRequest<Product>>(
      `${
        environment.API_URL +
        `/products?name=${name}&category=${category}&pageNumber=${paginationQuery.pageNumber}&pageSize=${paginationQuery.pageSize}`
      }`
    );
  }
}
