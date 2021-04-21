import { AlertService } from './../_services/alert.service';
import { RegexpFormulas } from './../_models/regexpFormulas';
import { AuthenticationService } from './../_services/authentication.service';
import { AthleteProductsService } from './../_services/athlete-products.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CategoryOfProduct } from './../_models/categoryOfProduct';
import { CategoriesService } from './../_services/categories.service';
import { PaginationQuery } from './../_models/paginationQuery';
import { ProductService } from './../_services/product.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Product } from '../_models/product';
import * as _ from 'lodash';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Jwt } from '../_models/jwt';

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
  closeResult = '';
  modalProduct: Product;
  copyModalProduct: Product;

  addProduct: FormGroup;

  bodyText: string;

  currentUser: Jwt;

  constructor(
    private productService: ProductService,
    private categoriesService: CategoriesService,
    private modalService: NgbModal,
    private athleteProductService: AthleteProductsService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService
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

    this.authenticationService.jwt.subscribe((x) => (this.currentUser = x));
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

  open(content, product) {
    this.addProduct = new FormGroup({
      weight: new FormControl(100, [
        Validators.required,
        Validators.pattern(RegexpFormulas.number),
      ]),
    });

    this.modalProduct = new Product();
    this.modalProduct = product;
    this.copyModalProduct = JSON.parse(JSON.stringify(this.modalProduct));
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  calculate(event) {
    let weight = event.target.value;
    this.modalProduct = JSON.parse(JSON.stringify(this.copyModalProduct));

    let currentCalories = this.modalProduct.calories;
    let currentProteins = this.modalProduct.proteins;
    let currentCarbohydrates = this.modalProduct.carbohydrates;
    let currentFats = this.modalProduct.fats;
    currentCalories = (currentCalories * weight) / 100;
    currentProteins = (currentProteins * weight) / 100;
    currentCarbohydrates = (currentCarbohydrates * weight) / 100;
    currentFats = (currentFats * weight) / 100;

    this.modalProduct.calories = +currentCalories.toFixed(2);
    this.modalProduct.proteins = +currentProteins.toFixed(2);
    this.modalProduct.fats = +currentFats.toFixed(2);
    this.modalProduct.carbohydrates = +currentCarbohydrates.toFixed(2);
  }

  closeModal(modal) {
    console.log(this.modalProduct.id);

    this.athleteProductService
      .post(this.modalProduct.id, this.addProduct.value.weight)
      .subscribe(
        (data) => {
          this.alertService.success(
            `Product added.  To check go to your account products`
          );
          modal.close('Save click');
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
