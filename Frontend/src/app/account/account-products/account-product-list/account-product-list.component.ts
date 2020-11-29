import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { UserProductService } from './../../../shared/user-product.service';
import { Component, Input, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/user.service';


@Component({
  selector: 'app-account-product-list',
  templateUrl: './account-product-list.component.html',
  styleUrls: ['./account-product-list.component.css']
})
export class AccountProductListComponent implements OnInit {

  user: any;
  term: string;
  data: any;
  myDate: Date = new Date();

  // tslint:disable-next-line: max-line-length
  constructor(public userService: UserService, public userProductService: UserProductService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      res  => {
        this.user = res;
        this.userProductService.getAllUserProducts(this.user.id);
      },
      err => {
        console.log(err);
      }
    );
  }

 
  onDelete(id) {
    if (confirm('Are you sure to delete this product?')) {
      this.userProductService.deleteUserproduct(id).subscribe(
        (res: any) => {
          this.userProductService.getAllUserProducts(this.user.id);
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

  filter(date) {
    let dateOnly = (date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate());
    console.log(dateOnly);
    this.userProductService.getAllUserProductFromDay(this.user.id, dateOnly);
  }

}

