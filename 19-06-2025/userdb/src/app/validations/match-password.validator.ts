import { FormGroup, ValidationErrors } from '@angular/forms';

export function matchPasswordValidator(group: FormGroup): ValidationErrors | null {
  const pass = group.get('password')?.value;
  const confirm = group.get('confirmPassword')?.value;

  return pass === confirm ? null : { passwordMismatch: true };
}
