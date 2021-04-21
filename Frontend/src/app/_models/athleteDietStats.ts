import { DietStat } from './dietStat';
import { Athlete } from './athlete';

export class AthleteDietStats {
  athlete: Athlete;
  dietStats: {
    dietStat: DietStat;
  };
}
