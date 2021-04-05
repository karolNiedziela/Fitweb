import { CategoryOfProduct } from './../_models/categoryOfProduct';
import { CategoriesService } from './../_services/categories.service';
import { PageRequest } from './../_models/pageRequest';
import { PaginationQuery } from './../_models/paginationQuery';
import { ProductService } from './../_services/product.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Product } from '../_models/product';
import * as _ from 'lodash';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
})
export class ProductsComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  categories: CategoryOfProduct[] = [];
  totalItems = 0;
  paginationQuery = new PaginationQuery(1, 10);
  sizes: number[];
  searchText = '';
  category = '';
  onSearchChanged: Subject<string> = new Subject<string>();
  onSearchChangedSubscription: Subscription;

  constructor(
    private productService: ProductService,
    private categoriesService: CategoriesService
  ) {}

  ngOnInit(): void {
    this.sizes = [10, 15, 20];
    this.getProducts();
    this.onSearchChangedSubscription = this.onSearchChanged
      .pipe(debounceTime(1000), distinctUntilChanged())
      .subscribe((newText) => {
        this.searchText = newText;
        this.getProducts();
      });

    this.categoriesService
      .getCategories()
      .subscribe((categories) => (this.categories = categories));
  }

  ngOnDestroy(): void {
    this.onSearchChangedSubscription.unsubscribe();
  }

  getProducts() {
    this.productService
      .getProducts(this.searchText, this.category, this.paginationQuery)
      .subscribe(
        (response) => {
          this.products = response.items as Product[];
          this.totalItems = response.totalItems;
        },
        (error) => {
          console.log(error);
        }
      );
  }

  onPageChange(pageNumber) {
    this.paginationQuery.pageNumber = pageNumber;
    this.getProducts();
  }

  onPageSizeChange(event) {
    this.paginationQuery.pageSize = event.target.value;
    this.paginationQuery.pageNumber = 1;
    this.getProducts();
  }

  onCategoryChange(event) {
    console.log(event.target.value);
    this.category = event.target.value;
    this.getProducts();
  }
}
