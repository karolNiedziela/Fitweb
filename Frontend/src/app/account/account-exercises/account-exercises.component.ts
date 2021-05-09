import { AlertService } from './../../_services/alert.service';
import { UpdateExercise } from './../../_models/updateExercise';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DayService } from './../../_services/day.service';
import { AthleteExercisesService } from './../../_services/athlete-exercises.service';
import { AthleteExercise } from './../../_models/athleteExercise';
import { Component, OnInit } from '@angular/core';
import { Day } from 'src/app/_models/day';
import { RegexpFormulas } from 'src/app/_models/regexpFormulas';

@Component({
  selector: 'app-account-exercises',
  templateUrl: './account-exercises.component.html',
  styleUrls: ['./account-exercises.component.css'],
})
export class AccountExercisesComponent implements OnInit {
  athleteExercise: AthleteExercise;
  days: Day[] = [];

  dayName = 'Monday';
  selectedDay: string;

  updateExercise: FormGroup;
  closeResult = '';
  error = '';
  modalExercise: UpdateExercise;
  copyModalExercise: UpdateExercise;

  constructor(
    private athleteExercisesService: AthleteExercisesService,
    private dayService: DayService,
    private modalService: NgbModal,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.refreshData();

    this.dayService.get().subscribe((days) => (this.days = days));
  }

  filter(name): void {
    this.dayName = name;

    this.athleteExercisesService
      .getExercises(this.dayName)
      .subscribe((athleteExercise) => {
        this.athleteExercise = athleteExercise;
      });
  }

  open(content, exercises) {
    this.selectedDay = exercises.day;
    this.updateExercise = new FormGroup({
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
      day: new FormControl(this.selectedDay.toString()),
    });

    this.modalExercise = new UpdateExercise();
    this.modalExercise = exercises;
    this.copyModalExercise = JSON.parse(JSON.stringify(this.modalExercise));

    this.updateExercise.get('weight').setValue(this.modalExercise.weight);
    this.updateExercise
      .get('numberOfSets')
      .setValue(this.modalExercise.numberOfSets);
    this.updateExercise
      .get('numberOfReps')
      .setValue(this.modalExercise.numberOfReps);

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
    if (this.updateExercise.value.day === '') {
      this.updateExercise.value.day = this.days[0];
    }

    this.athleteExercisesService
      .put(
        this.modalExercise.exercise.id,
        this.updateExercise.value.weight,
        this.updateExercise.value.numberOfSets,
        this.updateExercise.value.numberOfReps,
        this.updateExercise.value.day.name
      )
      .subscribe(
        (data) => {
          this.alertService.info(
            `Exercise: ${this.modalExercise.exercise.name} updated.`
          );
          modal.close('Save click');
          this.refreshData();
        },
        (error) => {
          console.log(error);
        }
      );
  }

  openConfirmDialog(modal) {
    if (confirm('Are you sure to delete?')) {
      this.athleteExercisesService
        .delete(this.modalExercise.exercise.id)
        .subscribe(
          (response) => {
            this.alertService.warning(
              `Exercise: ${this.modalExercise.exercise.name} deleted.`
            );

            this.refreshData();
          },
          (error) => {
            console.log(error);
          }
        );

      modal.close('Delete click');
    }
  }

  refreshData(): void {
    this.athleteExercisesService
      .getExercises(this.dayName)
      .subscribe((athleteExercise) => {
        this.athleteExercise = athleteExercise;
      });
  }
}
