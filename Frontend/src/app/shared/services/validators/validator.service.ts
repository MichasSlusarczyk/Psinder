import { Injectable } from '@angular/core';
import { ValidationErrors, ValidatorFn, AbstractControl, Validator, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidatorService {

  constructor() { }

  regexValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any}  | null  => {
      if (!control.value) {
        return null;
      }
      const valid = regex.test(control.value);
      return valid ? null : error;
    };
  }

  matchValidator(
    matchTo: string, 
    reverse?: boolean
  ): ValidatorFn {
    return (control: AbstractControl): 
    ValidationErrors | null => {
      if (control.parent && reverse) {
        const c = (control.parent?.controls as any)[matchTo] as AbstractControl;
        if (c) {
          c.updateValueAndValidity();
        }
        return null;
      }
      return !!control.parent &&
        !!control.parent.value &&
        control.value === 
        (control.parent?.controls as any)[matchTo].value
        ? null
        : { matching: true };
    };
  }

  getPasswordValidators(): ValidatorFn[] {
    return [
      Validators.required,
      this.regexValidator(new RegExp('[0-9]+'), { number: true }),
      this.regexValidator(new RegExp('[A-Z]+'), { capital: true }),
      this.regexValidator(new RegExp('[a-z]+'), { small: true }),
      this.regexValidator(new RegExp('[!@#$&()\\-`.+,/]+'), { special: true }),
      Validators.minLength(8),
    ];
  }
}
