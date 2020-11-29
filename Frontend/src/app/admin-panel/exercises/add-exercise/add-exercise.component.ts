import { ExerciseService } from 'src/app/shared/exercise.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-exercise',
  templateUrl: './add-exercise.component.html',
  styleUrls: ['./add-exercise.component.css']
})
export class AddExerciseComponent implements OnInit {

  constructor(public exerciseService: ExerciseService) { }

  ngOnInit(): void {
  }

}
