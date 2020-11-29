import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DietgoalService {

  readonly BaseURI = 'https://localhost:44318/api';

  constructor(private formBuilder: FormBuilder, private http: HttpClient) { }

  formModel = this.formBuilder.group({
    UserId: null,
    TotalCalories: [null, [Validators.required, Validators.pattern('^[0-9]*$')]],
    Proteins: [null, [Validators.required, Validators.pattern('^[0-9]*$')]], 
    Carbohydrates: [null, [Validators.required, Validators.pattern('^[0-9]*$')]],
    Fats: [null, [Validators.required, Validators.pattern('^[0-9]*$')]]
  });

  getDietGoal() {
    return this.http.get(this.BaseURI + '/account/dietgoals');
  }

  addDietGoal() {
    var body = {
      TotalCalories: this.formModel.value.TotalCalories,
      Proteins: this.formModel.value.Proteins,
      Carbohydrates: this.formModel.value.Carbohydrates,
      Fats: this.formModel.value.Fats
    };

    return this.http.post(this.BaseURI + '/account/dietgoals', body);
  }

}
