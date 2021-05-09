import { Athlete } from './athlete';
import { Product } from './product';
export class AthleteProduct {
  id: number;
  athlete: Athlete;
  products: {
    product: Product;
    weight: number;
  };
  dateCreated: Date;
  dateUpdated: Date;
}
