import { ToastrService } from 'ngx-toastr';
import { ProductService } from 'src/app/shared/product.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-admin-edit-product',
  templateUrl: './admin-edit-product.component.html',
  styleUrls: ['./admin-edit-product.component.css']
})
export class AdminEditProductComponent implements OnInit {

  constructor(public productService: ProductService, private activatedRoute: ActivatedRoute, private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.productService.formData.id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.productService.formData.name = this.activatedRoute.snapshot.paramMap.get('name');
    this.productService.formData.calories = Number(this.activatedRoute.snapshot.paramMap.get('calories'));
    this.productService.formData.proteins = Number(this.activatedRoute.snapshot.paramMap.get('proteins'));
    this.productService.formData.carbohydrates = Number(this.activatedRoute.snapshot.paramMap.get('carbohydrates'));
    this.productService.formData.fats = Number(this.activatedRoute.snapshot.paramMap.get('fats'));
    this.productService.refreshList();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
      this.productService.formData = {
        id: 0,
        name: null,
        calories: null,
        proteins: null,
        carbohydrates: null,
        fats: null
      };
    }
  }

  edit(form: NgForm) {
    this.productService.putProduct().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Edited successfully', 'Fitweb');
        this.router.navigateByUrl('/admin-panel/products');
        this.productService.refreshList();
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }


}
