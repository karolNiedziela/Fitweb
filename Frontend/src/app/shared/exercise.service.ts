import { FormBuilder, Validators } from '@angular/forms';
import { Exercise } from './../models/exercise.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ExerciseService {

  formData: Exercise = {
    id: 0,
    partOfBody: '',
    name: ''
  };

  formModel = this.formBuilder.group({
    PartOfBody: ['', Validators.required],
    Name: [null, [Validators.required]]
  });

  readonly rootURL = 'https://localhost:44318/api';
  list: Exercise[] = [];

  constructor(private http: HttpClient, private formBuilder: FormBuilder) { }

  refreshList() {
    return this.http.get(this.rootURL + '/exercises')
    .toPromise()
    .then(res => this.list = res as Exercise[]).then(res => res.sort((a, b) => (a > b ? 1 : -1)));
  }

  deleteExercise(id) {
    return this.http.delete(this.rootURL + '/exercises/' + id, id);
  }
}
