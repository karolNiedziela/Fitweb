import { FormBuilder, Validators } from '@angular/forms';
import { Product } from './../models/product.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  formData: Product =
  {
    id: 0,
    name: null,
    calories: null,
    proteins: null,
    carbohydrates: null,
    fats: null
  };

  formModel = this.formBuilder.group({
    Name: ['', Validators.required],
    Calories: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    Proteins: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    Carbohydrates: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    Fats: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
  });

  readonly rootURL = 'https://localhost:44318/api';
  list: Product[] = [];
  
  constructor(private httpClient: HttpClient, private formBuilder: FormBuilder) { }

  refreshList() {
    return this.httpClient.get(this.rootURL + '/products')
    .toPromise()
    .then(res => this.list = res as Product[]).then(res => res.sort((a, b) => (a > b ? 1 : -1)));
  }

  addProduct() {
    var body = {
      Name: this.formModel.value.Name,
      Calories: this.formModel.value.Calories,
      Proteins: this.formModel.value.Proteins,
      Carbohydrates: this.formModel.value.Carbohydrates,
      Fats: this.formModel.value.Fats
    };

    return this.httpClient.post(this.rootURL + '/products', body);
  }

  putProduct() {
    // tslint:disable-next-line: max-line-length
    return this.httpClient.put(this.rootURL + '/products',  {id: this.formData.id, name: this.formData.name, calories: this.formData.calories,
      proteins: this.formData.proteins, carbohydrates: this.formData.carbohydrates, fats: this.formData.fats});
  }

  deleteProduct(id) {
    return this.httpClient.delete(this.rootURL + '/products/' + id, id);
  }

 
}
