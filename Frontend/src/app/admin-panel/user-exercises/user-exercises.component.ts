import { UserExerciseService } from './../../shared/user-exercise.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserProductService } from 'src/app/shared/user-product.service';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-user-exercises',
  templateUrl: './user-exercises.component.html',
  styleUrls: ['./user-exercises.component.css']
})
export class UserExercisesComponent implements OnInit {
  term: string;
  p: 1;

  constructor(public userService: UserService, public userExerciseService: UserExerciseService, private toastr: ToastrService,
              private router: Router) { }

  ngOnInit(): void {
    this.userExerciseService.getUsersExercises();
  }

}
