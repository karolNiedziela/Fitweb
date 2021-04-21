import { RegexpFormulas } from './../_models/regexpFormulas';
import { AlertService } from './../_services/alert.service';
import { AthleteExercisesService } from './../_services/athlete-exercises.service';
import { DayService } from './../_services/day.service';
import { AuthenticationService } from './../_services/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PartOfBody } from './../_models/partOfBody';
import { PartOfBodyService } from './../_services/part-of-body.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';
import { PaginationQuery } from './../_models/paginationQuery';
import { ExerciseService } from './../_services/exercise.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Exercise } from '../_models/exercise';
import { ModalService } from 'ng-bootstrap-modal';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Jwt } from '../_models/jwt';
import { Day } from '../_models/day';
import { RouterLink } from '@angular/router';

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
  days: Day[] = [];

  modalExercise: Exercise;
  closeResult = '';
  addExercise: FormGroup;
  error = '';

  currentUser: Jwt;

  constructor(
    private exerciseService: ExerciseService,
    private partOfBodyService: PartOfBodyService,
    private modalService: NgbModal,
    private authenticationService: AuthenticationService,
    private dayService: DayService,
    private athleteExeciseService: AthleteExercisesService,
    private alertService: AlertService
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

    this.authenticationService.jwt.subscribe((x) => (this.currentUser = x));

    this.dayService.get().subscribe((days) => {
      this.days = days;
    });
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

  open(content, exercise) {
    this.addExercise = new FormGroup({});

    this.modalExercise = exercise;
    this.addExercise = new FormGroup({
      weight: new FormControl('', [
        Validators.required,
        Validators.pattern(RegexpFormulas.number),
      ]),
      numberOfSets: new FormControl('', [
        Validators.required,
        Validators.pattern(RegexpFormulas.number),
      ]),
      numberOfReps: new FormControl('', [
        Validators.required,
        Validators.pattern(RegexpFormulas.number),
      ]),
      day: new FormControl(''),
    });
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  closeModal(modal) {
    if (this.addExercise.value.day === '') {
      this.addExercise.value.day = this.days[0];
    }

    this.athleteExeciseService
      .post(
        this.modalExercise.id,
        this.addExercise.value.weight,
        this.addExercise.value.numberOfSets,
        this.addExercise.value.numberOfReps,
        this.addExercise.value.day.name
      )
      .subscribe(
        (data) => {
          this.alertService.success(
            `Exercise added. To check go to your account exercises`
          );
          modal.close();
        },
        (error) => {
          console.log(error);
          this.error = error;
        }
      );
  }
}
