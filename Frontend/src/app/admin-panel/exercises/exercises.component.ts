import { ToastrService } from 'ngx-toastr';
import { ExerciseService } from './../../shared/exercise.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-exercises',
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.css']
})
export class ExercisesComponent implements OnInit {

  term: string;
  p: 1;

  constructor(public exerciseService: ExerciseService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.exerciseService.refreshList();
  }

  onDelete(id) {
    if (confirm('Are you sure to delete this product?')) {
      this.exerciseService.deleteExercise(id).subscribe(
        (res: any) => {
          this.exerciseService.refreshList();
          this.toastr.warning('Deleted successfully.', 'Fitweb');

        },
        err => {
          console.log(err);
        }
      );
    }
  }
}

