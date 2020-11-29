import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-bmr-calculator',
  templateUrl: './bmr-calculator.component.html',
  styleUrls: ['./bmr-calculator.component.css']
})
export class BmrCalculatorComponent implements OnInit {

  Gender: any = ['Male', 'Female'];
  Intensity: any = ['Light', 'Moderate', 'High', 'Extremely'];

  formModel = this.formBuilder.group({
    GenderName: ['', Validators.required],
    Age: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    Weight: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    Height: [null, [Validators.required, Validators.pattern('^[0-9.]+$')]],
    IntensityName: ['', Validators.required]
  });

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
  }



  bmi () {
    var weight = this.formModel.value.Weight;
    var height = this.formModel.value.Height;
    var age = this.formModel.value.Age;
    var gender = this.formModel.value.GenderName;
    var intensity = this.formModel.value.IntensityName;

    var result = this.formModel.value.Weight / ((this.formModel.value.Height / 100) * (this.formModel.value.Height / 100));
    var roundedResult = result.toFixed(2);
    var explanation = document.getElementById('explanation');

    if (gender == 'Male') {
      var bmr = Math.round(this.bmrMen(weight, height, age));
      var caloriesPerDay = Math.round(this.calories(bmr, intensity.toString()));
    }
    else if (gender == 'Female') {
      var bmr = Math.round(this.bmrWomen(weight, height, age));
      var caloriesPerDay = Math.round(this.calories(bmr, intensity.toString()));
    }

    document.getElementById('result').innerHTML = 'Your bmi score is : ' + roundedResult.toString()
    + ' and you must consume ' + caloriesPerDay + ' calories to maintain your current weight';
    explanation.innerHTML = 'Explanation: ';

    if (result < 18.5) {
      explanation.innerHTML += 'It means that you have underweight';
    }
    else if (result >= 18.5 && result <= 24.9) {
      explanation.innerHTML += 'It means that you have normal weight';
    }
    else if (result >= 25 && result <= 29.9) {
      explanation.innerHTML += 'It means that you have overweight';
    }
    else {
      explanation.innerHTML += 'It means that you have obesity';
    }
    }

    bmrWomen(weight, height, age) {
      var bmr = 665 + (4.35 * weight) + (4.7 * height) - (4.7 * age);
      return bmr;
    }

    bmrMen(weight, height, age) {
      var bmr = 665 + (4.35 * weight) + (4.7 * height) - (4.7 * age);
      return bmr;
    }

    calories(bmr, level) {
      if (level =='Light'){
        return bmr * 1.375;
      }
    
      else if (level == 'Moderate'){
        return bmr * 1.55;
      }
    
      else if (level == 'High'){
        return bmr * 1.725;
      }
    
      else if (level == 'Extremely'){
        return bmr * 1.9;
      }
    }

}