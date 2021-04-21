import { DayService } from './../../_services/day.service';
import { AthleteExercisesService } from './../../_services/athlete-exercises.service';
import { AthleteExercise } from './../../_models/athleteExercise';
import { Component, OnInit } from '@angular/core';
import { Day } from 'src/app/_models/day';

@Component({
  selector: 'app-account-exercises',
  templateUrl: './account-exercises.component.html',
  styleUrls: ['./account-exercises.component.css'],
})
export class AccountExercisesComponent implements OnInit {
  athleteExercise: AthleteExercise;
  days: Day[] = [];

  dayName = 'Monday';

  constructor(
    private athleteExercisesService: AthleteExercisesService,
    private dayService: DayService
  ) {
    this.athleteExercisesService
      .getExercises(this.dayName)
      .subscribe((athleteExercise) => {
        this.athleteExercise = athleteExercise;
        console.log(this.athleteExercise);
      });

    this.dayService.get().subscribe((days) => (this.days = days));
  }

  ngOnInit(): void {}

  filter(name): void {
    this.dayName = name;

    console.log(this.dayName);

    this.athleteExercisesService
      .getExercises(this.dayName)
      .subscribe((athleteExercise) => {
        this.athleteExercise = athleteExercise;
        console.log(this.athleteExercise);
      });
  }
}
