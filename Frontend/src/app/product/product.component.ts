import { ProductService } from './../shared/product.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  term: string;
  p: 1;



  constructor(public productService: ProductService) { }

  ngOnInit(): void {
    this.productService.refreshList();
  }

 

}
