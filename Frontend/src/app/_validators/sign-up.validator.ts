import { AbstractControl, FormGroup } from '@angular/forms';

export function ComparePasswords(
  control: AbstractControl
): { [key: string]: any | null } {
  const password = control.get('password').value;
  const matchingControl = control.get('confirmPassword').value;

  return password === matchingControl ? null : { notSame: true };
}
