import {Component, EventEmitter, forwardRef, Input, OnInit, Output, ViewChild} from '@angular/core';
import * as moment from 'moment';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from '@angular/forms';

@Component({
  selector: 'app-input-date',
  templateUrl: './input-date.component.html',
  styleUrls: ['./input-date.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: forwardRef(() => InputDateComponent)
    }
  ]
})
export class InputDateComponent implements ControlValueAccessor, OnInit {
  @Input() disabled = false;
  @Input() readonly = false;
  @Input() monthSelector = false;
  @Input() yearSelector = false;
  @Input() minDate: Date | null;
  @Input() maxDate: Date | null;
  @Input() showTime = false;
  @Input() placeholder = '';
  @Input() appendTo ='';

  // tslint:disable-next-line:no-output-on-prefix
  @Output() onChange: EventEmitter<Date | null> = new EventEmitter<Date | null>();
  hasFocus = false;
  
  format: string;
  mask: string;

  _prevValue: Date | null = null;
  _maskedInputValue: string;
  _calendarValue: Date | null;

  get yearRange(): string {
    if (!this.minDate || !this.maxDate) {
      return '';
    }
    const minYear = this.minDate.getFullYear();
    const maxYear = this.maxDate.getFullYear();

    return `${minYear}:${maxYear}`;
  }

  onChangeFn: (_: any) => void = () => {
  }
  onTouchedFn: () => void = () => {
  }

  get maskedInputValue() {
    return this._maskedInputValue;
  }

  set maskedInputValue(value: string) {
    if (!this.hasFocus) {
      return;
    }

    this._maskedInputValue = value;
  }

  get calendarValue() {
    return this._calendarValue;
  }

  set calendarValue(value: Date | null) {
    this._calendarValue = value;
    const date = moment(value);
    this._maskedInputValue = date.isValid() ? date.format(this.format) : null;
  }

  ngOnInit() {
    let format = 'DD.MM.YYYY';
    let mask = '99.99.9999';

    if (this.showTime) {
      format += ' HH:mm';
      mask += ' 99:99';
    }

    this.format = format;
    this.mask = mask;
  }

  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  writeValue(obj: any): void {
    this.calendarValue = obj;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onFocus() {
    this.saveCurrentValue();
    this.hasFocus = true;
  }

  onBlur() {
    const date = moment(this.maskedInputValue, this.format);
    this.calendarValue = this.isValid(date) ? date.toDate() : null;
    this.notifyIfChanged(this.calendarValue);
    this.hasFocus = false;
    this.onTouchedFn();
  }

  notifyIfChanged(newDate: Date | null) {
    if (!this.areSame(newDate, this._prevValue)) {
      this.onChange.emit(newDate);
      this.onChangeFn(newDate);
    }
  }

  isValid(date: moment.Moment) {
    if (!date.isValid()) {
      return false;
    }

    if (this.minDate && date.isBefore(moment(this.minDate), 'minute')) {
      return false;
    }

    if (this.maxDate && date.isAfter(moment(this.maxDate), 'minute')) {
      return false;
    }

    return true;
  }

  onCalendarOpen() {
    this.saveCurrentValue();
  }

  onSelect(date: Date) {
    this.calendarValue = date;
    this.notifyIfChanged(date);
    this.onTouchedFn();
  }

  saveCurrentValue() {
    this._prevValue = this.calendarValue;
  }

  areSame(first: Date | null, second: Date | null) {
    if (!first && !second) {
      return true;
    }

    if ((first && !second) || (!first && second)) {
      return false;
    }

    return moment(first).isSame(moment(second), 'minute');
  }

  setValue(date: Date | null) {
    this.onCalendarOpen();
    this.onSelect(date);
  }
}
