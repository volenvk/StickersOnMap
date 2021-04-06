import {Component, forwardRef, Input} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";
import * as moment from 'moment';

@Component({
  selector: 'app-input-time',
  template: `
    <p-inputMask mask="99:99" placeholder="ЧЧ:ММ" [disabled]="disabled" [(ngModel)]="formValue"
                 (onFocus)="onFocus()" (onBlur)="onBlur()"></p-inputMask>`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => InputTimeComponent)
    }
  ]
})
export class InputTimeComponent implements ControlValueAccessor {

  @Input() disabled = false;
  timeFormat = 'HH:mm';
  hasFocus = false;

  _formValue: string;

  get formValue() {
    return this._formValue;
  }

  set formValue(value: string) {
    if (!this.hasFocus) {
      return;
    }

    this._formValue = value;

    this.onChange(value);
    this.onTouched(value);
  }

  onChange: (_: any) => void = () => {
  };
  onTouched: (_: any) => void = () => {
  };

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  writeValue(obj: any): void {
    this._formValue = obj;
  }

  onFocus() {
    this.hasFocus = true;
  }

  onBlur() {
    const time = moment(this.formValue, this.timeFormat);
    this.formValue = time.isValid() ? time.format(this.timeFormat) : null;
    this.hasFocus = false;
  }
}
