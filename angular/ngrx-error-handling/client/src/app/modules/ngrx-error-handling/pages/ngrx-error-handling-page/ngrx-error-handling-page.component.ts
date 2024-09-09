import { Component, inject } from '@angular/core';
import { Store, StoreModule } from '@ngrx/store';
import { LocalStore, ErrorHandlingActions as a } from '../../store';

@Component({
  standalone: true,
  imports: [StoreModule],
  templateUrl: './ngrx-error-handling-page.component.html',
})
export class NgrxErrorHandlingPageComponent {
  store = inject(Store<LocalStore>);
  throwException(): void {
    console.log('click');
    this.store.dispatch(a.loadData({ value: 1 }));
  }
}
