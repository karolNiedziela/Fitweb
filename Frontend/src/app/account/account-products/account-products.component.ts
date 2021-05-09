import { AlertService } from './../../_services/alert.service';
import { UpdateProduct } from './../../_models/updateProduct';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AthleteDietStats } from './../../_models/athleteDietStats';
import { AthleteDietstatsService } from './../../_services/athlete-dietstats.service';
import { CalendarService } from './../../_services/calendar.service';
import { AthleteProduct } from './../../_models/athleteProduct';
import { AthleteProductsService } from './../../_services/athlete-products.service';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Calendar } from 'src/app/_models/calendar';
import { DietStat } from 'src/app/_models/dietStat';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RegexpFormulas } from 'src/app/_models/regexpFormulas';

@Component({
  selector: 'app-account-products',
  templateUrl: './account-products.component.html',
  styleUrls: ['./account-products.component.css'],
})
export class AccountProductsComponent implements OnInit {
  athleteProducts: AthleteProduct;
  calendar: Calendar;
  athleteDietStats: AthleteDietStats;
  updateProduct: FormGroup;

  modalProduct: UpdateProduct;
  copyModalProduct: AthleteProduct;

  closeResult = '';

  constructor(
    private athleteProductsService: AthleteProductsService,
    private calendarService: CalendarService,
    private athleteDietStatsService: AthleteDietstatsService,
    private modalService: NgbModal,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.athleteProductsService.getProducts(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteProducts = response;
      },
      (error) => {
        console.log(error);
      }
    );

    this.calendarService.calendar.subscribe(
      (calendar) => (this.calendar = calendar)
    );

    this.athleteDietStatsService.getDietStats(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteDietStats = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getDateFormat(): string {
    let year = this.calendarService.calendarValue.year.toString();
    let month = (this.calendarService.calendarValue.month + 1).toString();
    if (+month < 10) {
      month = 0 + month;
    }
    let day = this.calendarService.calendarValue.dayNumber.toString();
    if (+day < 10) {
      day = 0 + day;
    }
    return `${year}-${month}-${day}`;
  }

  get() {
    console.log(this.getDateFormat());
    this.athleteProductsService.getProducts(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteProducts = response;
      },
      (error) => {
        console.log(error);
      }
    );

    this.athleteDietStatsService.getDietStats(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteDietStats = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  open(content, products) {
    this.updateProduct = new FormGroup({
      weight: new FormControl(100, [
        Validators.required,
        Validators.pattern(RegexpFormulas.number),
      ]),
    });

    this.modalProduct = products;
    this.copyModalProduct = JSON.parse(JSON.stringify(this.modalProduct));

    this.updateProduct.get('weight').setValue(this.modalProduct.weight);
    this.calculate(this.modalProduct.weight);

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

  calculateOnChange(event) {
    let weight = event.target.value;

    if (weight < 0) {
      return;
    }
    this.modalProduct = JSON.parse(JSON.stringify(this.copyModalProduct));
    this.calculate(weight);
  }

  calculate(weight) {
    let currentCalories = this.modalProduct.product.calories;
    let currentProteins = this.modalProduct.product.proteins;
    let currentCarbohydrates = this.modalProduct.product.carbohydrates;
    let currentFats = this.modalProduct.product.fats;

    currentCalories = (currentCalories * weight) / 100;
    currentProteins = (currentProteins * weight) / 100;
    currentCarbohydrates = (currentCarbohydrates * weight) / 100;
    currentFats = (currentFats * weight) / 100;

    this.modalProduct.product.calories = +currentCalories.toFixed(2);
    this.modalProduct.product.proteins = +currentProteins.toFixed(2);
    this.modalProduct.product.fats = +currentFats.toFixed(2);
    this.modalProduct.product.carbohydrates = +currentCarbohydrates.toFixed(2);
  }

  closeModal(modal) {
    this.athleteProductsService
      .put(
        this.modalProduct.product.id,
        this.modalProduct.id,
        this.updateProduct.value.weight
      )
      .subscribe(
        (data) => {
          this.alertService.info(
            `Product: ${this.modalProduct.product.name} updated.`
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
      this.athleteProductsService
        .delete(this.modalProduct.product.id, this.modalProduct.id)
        .subscribe(
          (response) => {
            this.alertService.warning(
              `Product: ${this.modalProduct.product.name} deleted.`
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
    this.athleteProductsService.getProducts(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteProducts = response;
      },
      (error) => {
        console.log(error);
      }
    );

    this.athleteDietStatsService.getDietStats(this.getDateFormat()).subscribe(
      (response) => {
        this.athleteDietStats = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
