import { ToastrService } from 'ngx-toastr';
import { ExerciseService } from 'src/app/shared/exercise.service';
import { UserService } from 'src/app/shared/user.service';
import { UserExerciseService } from './../../../shared/user-exercise.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DayService } from 'src/app/shared/day.service';

@Component({
  selector: 'app-edit-exercise',
  templateUrl: './edit-exercise.component.html',
  styleUrls: ['./edit-exercise.component.css']
})
export class EditExerciseComponent implements OnInit {

  user;

  constructor(public userExerciseService: UserExerciseService, public userService: UserService, public exerciseService: ExerciseService,
              private toastr: ToastrService, private activatedRoute: ActivatedRoute, private router: Router, 
              public dayService: DayService) { }

  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      (res: any) => {
        this.user = res;
        this.userExerciseService.formData.id = this.user.id;
        this.userExerciseService.form.userId = this.user.id;
        this.userExerciseService.form.id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
        this.userExerciseService.form.exercise.id = Number(this.activatedRoute.snapshot.paramMap.get('exerciseId'));
        this.userExerciseService.form.exercise.name = this.activatedRoute.snapshot.paramMap.get('name');
        this.userExerciseService.form.numberOfSets = Number(this.activatedRoute.snapshot.paramMap.get('numberOfSets'));
        this.userExerciseService.form.numberOfReps = Number(this.activatedRoute.snapshot.paramMap.get('numberOfReps'));
        this.userExerciseService.form.weight = Number(this.activatedRoute.snapshot.paramMap.get('weight'));
        this.userExerciseService.form.day = 'Monday';
        this.exerciseService.refreshList();
        this.dayService.getAllDays();
        this.resetForm();
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
        createdAt: this.user.createdAt
      };
    }
  }

  edit(form: NgForm) {
    this.userExerciseService.putUserExercise().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Edited successfully', 'Fitweb');
        this.router.navigateByUrl('/account/account-exercises');
        this.userExerciseService.getUserExercises(this.user.username);
      },
      (err: any) => {
        this.toastr.error(err.error.message, 'Fitweb');
      }
    );
  }
}
