import { DayService } from './../../../shared/day.service';
import { UserExerciseService } from './../../../shared/user-exercise.service';
import { ExerciseService } from './../../../shared/exercise.service';
import { Component, OnInit } from '@angular/core';
import { Toast, ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';
import { NgForm } from '@angular/forms';
import { ThrowStmt } from '@angular/compiler';
import { typeWithParameters } from '@angular/compiler/src/render3/util';

@Component({
  selector: 'app-account-exercise',
  templateUrl: './account-exercise.component.html',
  styleUrls: ['./account-exercise.component.css']
})
export class AccountExerciseComponent implements OnInit {

  term: string;
  user;
  day: any;
  // tslint:disable-next-line: max-line-length
  constructor(public exerciseService: ExerciseService, public userExerciseService: UserExerciseService, public userService: UserService, private toastr: ToastrService,
              public dayService: DayService) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.user = res;
        this.userExerciseService.formData.id = this.user.id;
        this.userExerciseService.form.userId = this.user.id;
        this.exerciseService.refreshList();
        this.dayService.getAllDays();
        this.userExerciseService.form.exercise.id = 3;
        this.userExerciseService.form.day = 'Monday';
      },
      err => {
      console.log(err);
      }
    );
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
      this.userExerciseService.formData = {
        id: 0,
        exercises: [],
        products: [],
        username: this.user.username,
        email: this.user.email,
        role: this.user.role,
        createdAt: this.user.createdAt,
      };
    }
  }

  add(form: NgForm) {
    this.userExerciseService.postUserExercise().subscribe(
      (res: any) => {
        this.resetForm(form);
        this.toastr.success('Added successfully', 'Fitweb');
        this.userExerciseService.getUserExercises(this.user.username);
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }

}
