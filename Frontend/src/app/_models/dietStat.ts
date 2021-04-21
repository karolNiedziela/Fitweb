export class DietStat {
  totalCalories: number;
  proteins: number;
  carbohydrates: number;
  fats: number;
  dateCreated: string;

  constructor(
    totalCalories: number,
    proteins: number,
    carbohydrates: number,
    fats: number,
    dateCreated: string
  ) {
    this.totalCalories = totalCalories;
    this.proteins = proteins;
    this.carbohydrates = carbohydrates;
    this.fats = fats;
    this.dateCreated = dateCreated;
  }
}
