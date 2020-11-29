import { UserExercise } from './userExercise.model';
import { UserProduct } from './userProduct.model';

export class User {
    products: UserProduct[];
    exercises: UserExercise[];
    id: number;
    username: string;
    email: string;
    role: string;
    createdAt: Date;
}
