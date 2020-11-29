import { Exercise } from './exercise.model';
export class UserExercise {
    id: number;
    userId: number;
    exercise: Exercise;
    weight: number;
    numberOfSets: number;
    numberOfReps: number;
    day: string;
}