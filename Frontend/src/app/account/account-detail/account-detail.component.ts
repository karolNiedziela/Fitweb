import { DietgoalService } from './../../shared/dietgoal.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.css']
})
export class AccountDetailComponent implements OnInit {

  user;
  dietgoal: any;

  constructor(private router: Router, private service: UserService, private dietGoalService: DietgoalService) { }

  ngOnInit(): void {
    this.service.getUserAccount().subscribe(
      (res: any) => {
        this.user = res;
        this.dietGoalService.getDietGoal().subscribe(
          (res: any) => {
            this.dietgoal = res;
          },
          err => {
            console.log(err);
          }
        );
      },
      err => {
        console.log(err);
      }
    );
  }

}
