import { User } from './../models/user.model';
import { HttpClient } from '@angular/common/http';
import { UserExercise } from '../models/userExercise.model';
import { Injectable } from '@angular/core';
import { UserProduct } from '../models/userProduct.model';
import { Exercise } from '../models/exercise.model';

@Injectable({
  providedIn: 'root'
})
export class UserExerciseService {

  form: UserExercise = {
    id: null,
    userId: 0,
    exercise: new Exercise(),
    weight: null,
    numberOfSets: null,
    numberOfReps: null,
    day: ''
  };

  formData: User =
  {
    products: [],
    exercises: [],
    id: 0,
    username: '',
    email: '',
    role: '',
    createdAt: null
  };


  readonly rootURL = 'https://localhost:44318/api';

  constructor(private http: HttpClient) {
  }

  list: UserExercise[] = [];

  postUserExercise() {
    // tslint:disable-next-line: max-line-length
    return this.http.post(this.rootURL + '/userexercises', {userId: this.form.userId, exerciseId: this.form.exercise.id, weight: this.form.weight,
    numberOfSets: this.form.numberOfSets, numberOfReps: this.form.numberOfReps, day: this.form.day});
  }

  putUserExercise() {
    return this.http.put(this.rootURL + '/userexercises',  {id: this.form.id, userId: this.form.userId, exerciseId: this.form.exercise.id,
      numberOfSets: this.form.numberOfSets, numberOfReps: this.form.numberOfReps, weight: this.form.weight, day: this.form.day});
  }

  getUserExercises(username) {
    return this.http.get(this.rootURL + '/users/' + username)
    .toPromise()
    .then((res: any) => this.formData = res as User);
  }

  getUsersExercises() {
    return this.http.get(this.rootURL + '/userexercises')
    .toPromise()
    .then((res:any) => this.list = res as UserExercise[])
  }

  getAllUserExercises(id) {
    return this.http.get(this.rootURL + '/userExercises/' + id, id)
    .toPromise()
    .then((res: any) => this.list = res as UserExercise[]);
  }

  deleteUserExercise(id) {
    return this.http.delete(this.rootURL + '/userexercises/' + id, id);
  }
}
