import { Exercise } from './exercise';
import { Athlete } from './athlete';

export class AthleteExercise {
  athlete: Athlete;
  exercises: {
    exercise: Exercise;
    weight: number;
    numberOfSets: number;
    numberOfReps: number;
    day: string;
  };
  dateCreated: Date;
}
