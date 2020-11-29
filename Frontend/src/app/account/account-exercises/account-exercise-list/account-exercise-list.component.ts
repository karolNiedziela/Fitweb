import { DayService } from 'src/app/shared/day.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserExerciseService } from './../../../shared/user-exercise.service';
import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/user.service';
import { Exercise } from 'src/app/models/exercise.model';
import { ExerciseService } from 'src/app/shared/exercise.service';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-account-exercise-list',
  templateUrl: './account-exercise-list.component.html',
  styleUrls: ['./account-exercise-list.component.css']
})
export class AccountExerciseListComponent implements OnInit {

  user;
  term: string;
  selectedItem = null;
  p: 1;

  constructor(public exerciseService: ExerciseService, public userService: UserService, public userExerciseService: UserExerciseService,
              public dayService: DayService, private toastr: ToastrService, private router: Router) { }


  ngOnInit(): void {
    this.userService.getUserAccount().subscribe(
      res  => {
        this.user = res;
        this.userExerciseService.form.userId = this.user.id;
        this.userExerciseService.getUserExercises(this.user.username);
      },
      err => {
        console.log(err);
      }
    );
  }

  onDelete(id) {
    if (confirm('Are you sure to delete this product?')) {
      this.userExerciseService.deleteUserExercise(id).subscribe(
        (res: any) => {
          this.userExerciseService.getUserExercises(this.user.username);
          this.toastr.warning('Deleted successfully.', 'Fitweb');
        },
        err => {
          console.log(err);
          console.log(this.userExerciseService.form);
        }
      );
    }
  }

  edit(id) {
    this.router.navigateByUrl('/account/edit-product');
  }

  filter(name) {
    if (name == 'all') {
      this.term = ' ';
      this.selectedItem = name;
    } else
    {
      this.term = name;
      this.selectedItem = name;
    }

    
  }

}
