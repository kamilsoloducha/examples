import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  signal,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-custom-form-control',
  standalone: true,
  imports: [],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomFormControlComponent),
      multi: true,
    },
  ],
  templateUrl: './custom-form-control.component.html',
  styleUrl: './custom-form-control.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomFormControlComponent implements ControlValueAccessor {
  value = signal(0);
  isDisabled = signal(false);
  isTouched = false;
  onChange = (_: number) => {};
  onTouched = () => {};

  writeValue(obj: number): void {
    this.value.set(obj);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled.set(isDisabled);
  }

  increse(): void {
    this.value.update((curr) => curr + 1);
    this.onChange(this.value());
    this.markAsTouched();
  }

  decrease(): void {
    this.value.update((curr) => curr - 1);
    this.onChange(this.value());
    this.markAsTouched();
  }

  markAsTouched(): void {
    if (!this.isTouched) {
      this.isTouched = true;
      this.onTouched();
    }
  }
}
