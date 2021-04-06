import {InputTimeComponent} from "./input-time.component";

describe('input-time component', () => {
  it('should not have any value at initialization', () => {
    const sut = new InputTimeComponent();
    expect(sut.formValue).not.toBeTruthy();
  })

  it('should allow enter valid time', () => {
    const sut = new InputTimeComponent();
    sut.hasFocus = true;
    const expected = '22:46';

    sut.formValue = expected;

    expect(sut.formValue).toBe(expected);
  })

  it('should not set value when lost focus', () => {
    const sut = new InputTimeComponent();
    sut.hasFocus = false;

    sut.formValue = '12:30';

    expect(sut.formValue).not.toBeTruthy();
  })

  it('should clear invalid time on Blur when has invalid minutes', () => {
    const sut = new InputTimeComponent();
    sut.hasFocus = true;
    sut.formValue = '16:79';

    sut.onBlur();

    expect(sut.formValue).not.toBeTruthy();
  })

  it('should clear invalid time on Blur when has invalid hours', () => {
    const sut = new InputTimeComponent();
    sut.hasFocus = true;
    sut.formValue = '25:32';

    sut.onBlur();

    expect(sut.formValue).not.toBeTruthy();
  })

  it('should call onChange on value changes', () => {
    let changed = false;
    const sut = new InputTimeComponent();
    sut.hasFocus = true;
    sut.registerOnChange(() => {
      changed = true;
    });

    sut.formValue = '12:30';

    expect(changed).toBeTruthy();
  })

  it('should call onTouched on value changes', () => {
    let touched = false;
    const sut = new InputTimeComponent();
    sut.hasFocus = true;
    sut.registerOnTouched(() => {
      touched = true;
    });

    sut.formValue = '12:30';

    expect(touched).toBeTruthy();
  })
})
