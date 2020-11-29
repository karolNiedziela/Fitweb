import { UserProduct } from './../models/userProduct.model';
import { FormBuilder } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserProductService {
  form: UserProduct =
  {
    id: null,
    userId: 0,
    product: new Product(),
    weight: null,
    calories: null,
    proteins: null,
    carbohydrates: null,
    fats: null,
    addedAt: ''
  };

  formData: User =
  {
    products: [],
    exercises: [],
    id: 0,
    username: '',
    email: '',
    role: '',
    createdAt: null
  };


readonly rootURL = 'https://localhost:44318/api';

list: UserProduct[] = [];

  constructor(private formBuilder: FormBuilder, private http: HttpClient) { }

  getUserProducts(username) {
    return this.http.get(this.rootURL + '/users/' + username)
    .toPromise()
    .then((res: any) => this.formData = res as User);
  }

  getAllUsersProducts() {
    return this.http.get(this.rootURL + '/userproducts')
    .toPromise()
    .then((res: any) => this.list = res as UserProduct[]);
  }

  getAllUserProductFromDay(id, date) {
    let params = new HttpParams();
    params = params.append('userId', id);
    params = params.append('date', date);
    return this.http.get<any>(this.rootURL + '/userproducts/' + id + '/' + date, {params})
    .toPromise()
    .then((res: any) => this.list = res as UserProduct[]);
  }

  getAllUserProducts(id) {
    return this.http.get(this.rootURL + '/userproducts/' + id, id)
    .toPromise()
    .then((res: any) => this.list = res as UserProduct[]);
  }


  postUserProduct() {
    return this.http.post(this.rootURL + '/userproducts', {userId: this.form.userId, productId: this.form.product.id,
      weight: this.form.weight}).pipe(catchError(this.handleError));
  }

  putUserProduct() {
    return this.http.put(this.rootURL + '/userproducts',  {userId: this.form.userId, productId: this.form.product.id,
      weight: this.form.weight});
  }

  deleteUserproduct(id) {
    return this.http.delete(this.rootURL + '/userproducts/' + id, id);

  }

  handleError(error: HttpErrorResponse) {
    return throwError(error);
  }
}

