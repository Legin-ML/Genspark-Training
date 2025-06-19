import { AbstractControl, ValidationErrors } from '@angular/forms';

export function passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value || '';
  const minLength = value.length >= 6;

  return minLength
    ? null
    : { weakPassword: true };
}
