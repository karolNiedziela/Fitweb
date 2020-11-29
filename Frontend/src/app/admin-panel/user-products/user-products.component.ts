import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProductService } from 'src/app/shared/user-product.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-user-products',
  templateUrl: './user-products.component.html',
  styleUrls: ['./user-products.component.css']
})
export class UserProductsComponent implements OnInit {

  term: string;
  p: 1;

  constructor(public userService: UserService, public userProductService: UserProductService, private toastr: ToastrService,
              private router: Router) { }

  ngOnInit(): void {
    this.userProductService.getAllUsersProducts();
  }

}

