import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/shared/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  term: string;
  p: 1;

  constructor(public productService: ProductService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.productService.refreshList();
  }

  onDelete(id) {
    if (confirm('Are you sure to delete this product?')) {
      this.productService.deleteProduct(id).subscribe(
        (res: any) => {
          this.productService.refreshList();
          this.toastr.warning('Deleted successfully.', 'Fitweb');

        },
        err => {
          console.log(err);
        }
      );
    }
  }

  edit(id) {
    this.router.navigateByUrl('/account/edit-product');
  }
}
