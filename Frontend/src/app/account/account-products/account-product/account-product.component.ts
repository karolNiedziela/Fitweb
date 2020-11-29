import { UserService } from 'src/app/shared/user.service';
import { NgForm, FormGroup } from '@angular/forms';
import { UserProductService } from './../../../shared/user-product.service';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from './../../../shared/product.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-account-product',
  templateUrl: './account-product.component.html',
  styleUrls: ['./account-product.component.css']
})
export class AccountProductComponent implements OnInit {

  searchText: string;
  user;
  // tslint:disable-next-line: max-line-length
  constructor(public productService: ProductService, private toastr: ToastrService, public userProductService: UserProductService, public userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.user = res;
        this.userProductService.formData.id = this.user.id;
        this.userProductService.form.userId = this.user.id;
        this.productService.refreshList();
        this.userProductService.form.product.id = 2;
      },
      err => {
      console.log(err);
      }
    );
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
      this.userProductService.formData = {
        id: 0,
        products: [],
        exercises: [],
        username: this.user.username,
        email: this.user.email,
        role: this.user.role,
        createdAt: this.user.createdAt
      };
    }
  }

  add(form: NgForm) {
    this.userProductService.postUserProduct().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Added successfully', 'Fitweb');
        this.userProductService.getAllUserProducts(this.user.id);
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }

}
