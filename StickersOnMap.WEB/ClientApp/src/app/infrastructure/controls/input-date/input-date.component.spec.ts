import {InputDateComponent} from './input-date.component';
import * as moment from 'moment';

function createInputDateComponent(): InputDateComponent {
  const component = new InputDateComponent();
  component.hasFocus = true;
  return component;
}

describe('input time component', () => {
  it('should not have value at initialization', () => {
    const sut = createInputDateComponent();
    expect(sut.calendarValue).not.toBeTruthy();
  });

  it('should have correct format when time is not present', () => {
    const sut = createInputDateComponent();
    sut.showTime = false;

    sut.ngOnInit();

    expect(sut.format).toBe('DD.MM.YYYY');
  });

  it('should have correct format when time is present', () => {
    const sut = createInputDateComponent();
    sut.showTime = true;

    sut.ngOnInit();

    expect(sut.format).toBe('DD.MM.YYYY HH:mm');
  });

  it('should change input value when correct calendar value is set', () => {
    const sut = createInputDateComponent();
    const date = moment();
    const expected = date.format(sut.format);

    sut.onCalendarOpen();
    sut.onSelect(date.toDate());

    expect(sut.maskedInputValue).toBe(expected);
  });

  it('should change calendar value onBlur when correct value has been manually typed', () => {
    const sut = createInputDateComponent();
    const expected = moment().startOf('minute');

    sut.onFocus();
    sut.maskedInputValue = expected.format(sut.format);
    sut.onBlur();

    expect(sut.calendarValue).toEqual(expected.toDate());
  });


  it('should clear invalid date on Blur', () => {
    const sut = createInputDateComponent();
    sut.setValue(new Date());

    sut.onFocus();
    sut.maskedInputValue = '99.99.9999';
    sut.onBlur();

    expect(sut.calendarValue).not.toBeTruthy();
    expect(sut.maskedInputValue).not.toBeTruthy();
  });

  it('should emit onChange event when new value has been chosen in a calendar', () => {
    let emitted = false;
    const sut = createInputDateComponent();
    sut.onChange.subscribe(_ => emitted = true);

    sut.onCalendarOpen();
    sut.onSelect(new Date());

    expect(emitted).toBeTrue();
  });

  it('should not emit onChange event when the same value has been chosen in a calendar', () => {
    let emitted = false;
    const sut = createInputDateComponent();
    const date = moment().startOf('day').toDate();
    sut.setValue(date);
    sut.onChange.subscribe(_ => emitted = true);

    sut.onCalendarOpen();
    sut.onSelect(date);

    expect(emitted).toBeFalse();
  });

  it('should emit onChange event when a new value has been typed in the input', () => {
    let emitted = false;
    const sut = createInputDateComponent();
    sut.onChange.subscribe(_ => emitted = true);

    sut.onFocus();
    sut.maskedInputValue = moment().format(sut.format);
    sut.onBlur();

    expect(emitted).toBeTrue();
  });

  it('should not emit onChange event when the same value has been typed in the input', () => {
    let emitted = false;
    const sut = createInputDateComponent();
    const date = moment();
    sut.setValue(date.toDate());
    sut.onChange.subscribe(_ => emitted = true);

    sut.onFocus();
    sut.maskedInputValue = date.format(sut.format);
    sut.onBlur();

    expect(emitted).toBeFalse();
  });

  it('should not change masked input value when lost focus', () => {
    const sut = createInputDateComponent();
    const date = moment('2020-12-31');
    sut.setValue(date.toDate());
    sut.hasFocus = false;

    sut.maskedInputValue = moment('2021-01-01').format(sut.format);

    expect(sut.maskedInputValue).toBe(date.format(sut.format));
  });

  it('should set value by Angular when does not have focus', () => {
    const sut = createInputDateComponent();
    const date = moment();
    sut.hasFocus = false;

    sut.writeValue(date.toDate());

    expect(moment(sut.calendarValue).isSame(date)).toBeTrue();
    expect(sut.maskedInputValue).toBe(date.format(sut.format));
  });

  it('should not set date before min date', () => {
    const sut = createInputDateComponent();
    const minDate = moment('1990-01-01');
    sut.minDate = minDate.toDate();

    sut.onFocus();
    sut.maskedInputValue = minDate.subtract(1, 'day').format(sut.format);
    sut.onBlur();

    expect(sut.calendarValue).not.toBeTruthy();
    expect(sut.maskedInputValue).not.toBeTruthy();
  });

  it('should not set date after max date', () => {
    const sut = createInputDateComponent();
    const maxDate = moment('2020-12-31');
    sut.maxDate = maxDate.toDate();

    sut.onFocus();
    sut.maskedInputValue = maxDate.add(1, 'day').format(sut.format);
    sut.onBlur();

    expect(sut.calendarValue).not.toBeTruthy();
    expect(sut.maskedInputValue).not.toBeTruthy();
  });
});
