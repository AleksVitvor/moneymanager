import { AbstractControl, ValidationErrors } from '@angular/forms'

export function repeatableRequired(control: AbstractControl): ValidationErrors | null {
  const isRepetable = control.get("isRepeatable").value;
  const period = control.get("transactionPeriodId").value;

  if (isRepetable === true && (period === null || period === undefined)) {
    return { 'noMatch': true };
  }

  return null;
}
