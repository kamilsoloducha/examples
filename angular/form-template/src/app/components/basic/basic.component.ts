import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CustomFormControlComponent } from '../custom-form-control/custom-form-control.component';

@Component({
  selector: 'app-basic',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, CustomFormControlComponent],
  templateUrl: './basic.component.html',
  styleUrl: './basic.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BasicComponent {
  formValue = signal('');

  readonly form = new FormGroup<LoginForm>(
    {
      userName: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.minLength(5)],
      }),
      password: new FormControl<string>('', {
        nonNullable: true,
        validators: [Validators.minLength(5)],
      }),
      includeAddresses: new FormControl<boolean>(false, {
        nonNullable: true,
        updateOn: 'change',
      }),
      addresses: new FormArray<FormGroup<AddressForm>>([]),
      age: new FormControl<number>(1, {
        nonNullable: true,
        updateOn: 'submit',
      }),
    },
    { updateOn: 'submit' }
  );

  readonly userName = this.form.get('userName');
  readonly password = this.form.get('password');
  readonly includeAddresses = this.form.get('includeAddresses');
  readonly addresses = this.form.get('addresses') as FormArray;

  submit(): void {
    if (!this.form.valid || !this.form.touched) {
      return;
    }
    const formJsonfy = JSON.stringify(this.form.value);
    this.formValue.set(formJsonfy);
  }

  cleanForm(): void {
    this.form.setValue({
      userName: '',
      password: '',
      includeAddresses: false,
      addresses: [],
      age: 1,
    });
  }

  addAddress(): void {
    this.addresses.push(
      new FormGroup<AddressForm>({
        address: new FormControl(''),
        postcode: new FormControl(''),
      })
    );
  }

  stop($event: Event) {
    console.log($event);
    $event.preventDefault();
    $event.stopPropagation();
  }
}

interface LoginForm {
  userName: FormControl<string>;
  password: FormControl<string>;
  includeAddresses: FormControl<boolean>;
  addresses: FormArray<FormGroup<AddressForm>>;
  age: FormControl<number>;
}

interface AddressForm {
  address: FormControl<string | null>;
  postcode: FormControl<string | null>;
}
