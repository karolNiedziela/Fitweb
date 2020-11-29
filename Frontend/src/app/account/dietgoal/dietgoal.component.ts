import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';
import { DietgoalService } from './../../shared/dietgoal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dietgoal',
  templateUrl: './dietgoal.component.html',
  styleUrls: ['./dietgoal.component.css'],
})
export class DietgoalComponent implements OnInit {
  user;

  constructor(
    public dietGoalService: DietgoalService,
    public userService: UserService,
    public toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.dietGoalService.formModel.reset();
        this.user = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  onSubmit() {
    this.dietGoalService.addDietGoal().subscribe(
      (res: any) => {
        this.dietGoalService.formModel.reset();
        this.toastr.success('Your diet goals added succesfully', 'Fitweb');
        this.router.navigateByUrl('/account/account-detail');
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }
}
