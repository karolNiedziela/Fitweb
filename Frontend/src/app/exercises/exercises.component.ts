import { PartOfBody } from './../_models/partOfBody';
import { PartOfBodyService } from './../_services/part-of-body.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';
import { PaginationQuery } from './../_models/paginationQuery';
import { ExerciseService } from './../_services/exercise.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Exercise } from '../_models/exercise';

@Component({
  selector: 'app-exercises',
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.css'],
})
export class ExercisesComponent implements OnInit, OnDestroy {
  exercises: Exercise[] = [];
  parts: PartOfBody[] = [];
  paginationQuery = new PaginationQuery(1, 10);
  totalItems = 0;
  sizes: number[];
  searchText = '';
  part = '';
  onSearchChanged: Subject<string> = new Subject<string>();
  onSearchChangedSubscription: Subscription;

  constructor(
    private exerciseService: ExerciseService,
    private partOfBodyService: PartOfBodyService
  ) {}

  ngOnInit(): void {
    this.sizes = [10, 15, 20];
    this.getExercises();

    this.onSearchChangedSubscription = this.onSearchChanged
      .pipe(debounceTime(1000), distinctUntilChanged())
      .subscribe((newText) => {
        this.searchText = newText;
        this.getExercises();
      });

    this.partOfBodyService
      .getParts()
      .subscribe((parts) => (this.parts = parts));
  }

  ngOnDestroy(): void {
    this.onSearchChangedSubscription.unsubscribe();
  }

  getExercises() {
    this.exerciseService
      .getExercises(this.searchText, this.part, this.paginationQuery)
      .subscribe((response) => {
        this.exercises = response.items;
        this.totalItems = response.totalItems;
      });
  }

  onPageChange(pageNumber) {
    this.paginationQuery.pageNumber = pageNumber;
    this.getExercises();
  }

  onPageSizeChange(event) {
    this.paginationQuery.pageSize = event.target.value;
    this.getExercises();
  }

  onPartChange(event) {
    this.part = event.target.value;
    this.getExercises();
  }
}
