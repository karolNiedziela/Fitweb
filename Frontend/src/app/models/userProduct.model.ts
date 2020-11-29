import { Validators } from '@angular/forms';
import { Product } from './product.model';

export class UserProduct {
    id: number;
    userId: number;
    product: Product;
    weight: number;
    calories: number;
    proteins: number;
    carbohydrates: number;
    fats: number;
    addedAt: string;
}
