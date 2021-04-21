import { Athlete } from './athlete';
import { Product } from './product';
export class AthleteProduct {
  athlete: Athlete;
  products: {
    product: Product;
    weight: number;
  };
  dateCreated: Date;
}
