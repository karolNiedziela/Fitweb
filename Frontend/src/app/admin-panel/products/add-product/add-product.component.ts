import { ToastrService } from 'ngx-toastr';
import { ProductService } from './../../../shared/product.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  

  constructor(public productService: ProductService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.productService.formModel.reset();
  }

  onSubmit() {
    this.productService.addProduct().subscribe(
     (res: any) => {
      this.productService.formModel.reset();
      this.toastr.success('New user created!', 'Registration succesful.');
     },
    (err: any) => {
        this.toastr.error(err.error.message, 'Registration failed');
     }
    );
  }

}
