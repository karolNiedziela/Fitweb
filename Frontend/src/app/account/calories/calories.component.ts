import { DietgoalService } from './../../shared/dietgoal.service';
import { AccountService } from 'src/app/shared/account.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-calories',
  templateUrl: './calories.component.html',
  styleUrls: ['./calories.component.css']
})
export class CaloriesComponent implements OnInit {

  dailyValues: any;
  diet: any;

  constructor(private router: Router, private service: UserService, private accountService: AccountService,
              private dietGoalService: DietgoalService) { }

  ngOnInit(): void {
    this.accountService.getCalories().subscribe(
      (res: any) => {
        this.dailyValues = res;
        this.dietGoalService.getDietGoal().subscribe(
          (result: any) => {
            this.diet = result;
          },
          (error: any) => {
            console.log(error);
          }
        );
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  check() {
    var totalCalories = document.getElementById('totalCalories');
    var proteins = document.getElementById('proteins');
    var carbo = document.getElementById('carbo');
    var fats = document.getElementById('fats');
    if (this.dailyValues?.calories > this.diet?.totalCalories) {
      totalCalories.style.color = 'red';
    } else {
      totalCalories.style.color = 'green';
    }
    if (this.dailyValues?.proteins > this.diet?.proteins) {
      proteins.style.color = 'red';
    } else {
      proteins.style.color = 'green';
    }
    if (this.dailyValues?.carbohydrates > this.diet?.carbohydrates) {
      carbo.style.color = 'red';
    } else {
      carbo.style.color = 'green';
    }
    if (this.dailyValues?.fats > this.diet?.fats) {
      fats.style.color = 'red';
    } else {
      fats.style.color = 'green';
    }

  }
}
