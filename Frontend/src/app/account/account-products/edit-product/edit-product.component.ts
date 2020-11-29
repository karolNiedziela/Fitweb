import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/shared/product.service';
import { UserProductService } from 'src/app/shared/user-product.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  user;
  productId: any;

  constructor(public productService: ProductService, private toastr: ToastrService, public userProductService: UserProductService,
              public userService: UserService, private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.user = res;
        this.userProductService.formData.id = this.user.id;
        this.userProductService.form.userId = this.user.id;
        this.userProductService.form.product.id = Number(this.activatedRoute.snapshot.paramMap.get('productId'));
        this.userProductService.form.product.name = this.activatedRoute.snapshot.paramMap.get('name');
        this.productService.refreshList();
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

  edit(form: NgForm) {
    this.userProductService.putUserProduct().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Edited successfully', 'Fitweb');
        this.router.navigateByUrl('/account/account-products');
        this.userProductService.getUserProducts(this.user.username);
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }

}
