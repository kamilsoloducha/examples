import { Component, inject } from '@angular/core';
import {
  HttpOperatorsState,
  HttpOperatorsActions as a,
  selectValues,
} from './store';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-http-operators',
  templateUrl: './http-operators.component.html',
})
export class HttpOperatorsComponent {
  private readonly store = inject(Store<HttpOperatorsState>);
  values$ = this.store.select(selectValues);

  switchMapLoad(): void {
    this.store.dispatch(a.loadDataSwitchMap({ timeout: 5 }));
  }

  concatMapLoad(): void {
    this.store.dispatch(a.loadDataConcatMap({ timeout: 5 }));
  }

  mergeMapLoad(): void {
    this.store.dispatch(a.loadDataMergeMap({ timeout: 1 }));
  }
  exhaustMapLoad(): void {
    this.store.dispatch(a.loadDataExhaustMap({ timeout: 5 }));
  }
}
