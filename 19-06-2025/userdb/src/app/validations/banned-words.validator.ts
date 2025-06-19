import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function bannedWordsValidator(words: string[]): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value?.toLowerCase() || '';
    const hasBanned = words.some(word => value.includes(word));
    return hasBanned ? { bannedWord: true } : null;
  };
}
