import { ExerciseService } from './../shared/exercise.service';
import { Component, OnInit } from '@angular/core';
import { timestamp } from 'rxjs/operators';

@Component({
  selector: 'app-exercise',
  templateUrl: './exercise.component.html',
  styleUrls: ['./exercise.component.css']
})
export class ExerciseComponent implements OnInit {
  term: string;
  p: 1;
  selectedItem = null;
  PartOfBody: any = ['Abs', 'Biceps', 'Triceps', 'Chest', 'Back', 'Legs'];

  constructor(public exerciseService: ExerciseService) { }

  ngOnInit(): void {
    this.exerciseService.refreshList();
  }

  filter(part) {
    if (part == 'all') {
      this.term = ' ';
      this.selectedItem = part;
    } else
    {
      this.term = part;
      this.selectedItem = part;
    }
  }
}

