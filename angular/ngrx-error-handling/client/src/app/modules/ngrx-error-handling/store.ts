import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  createActionGroup,
  createFeature,
  createReducer,
  on,
  props,
} from '@ngrx/store';
import {
  catchError,
  map,
  MonoTypeOperatorFunction,
  Observable,
  ObservableInput,
  ObservedValueOf,
  OperatorFunction,
  startWith,
  switchMap,
  tap,
} from 'rxjs';
import { BackendHttpService } from './backend-http.service';
import { Action } from 'rxjs/internal/scheduler/Action';

export type LocalStore = {
  isLoading: boolean;
};

export const initState: LocalStore = {
  isLoading: false,
};

export const ErrorHandlingActions = createActionGroup({
  source: 'ErrorHandling',
  events: {
    'Load Data': props<{ value: number }>(),
    'Set Data': props<{ value: number | undefined }>(),
    'Handle Error': props<{ error: string }>(),
  },
});

export const errorHandlingFeature = createFeature({
  name: 'errorHandling',
  reducer: createReducer(
    initState,
    on(ErrorHandlingActions.loadData, (_state, { value }) => {
      return { ..._state, isLoading: true };
    }),
    on(ErrorHandlingActions.setData, (_state, { value }) => {
      return { ..._state, isLoading: false };
    })
  ),
});

export class LocalStoreEffects {
  actions$ = inject(Actions);
  backendService = inject(BackendHttpService);

  loadData$ = createEffect(() =>
    this.actions$.pipe(
      tap((_) => console.log('effect')),
      ofType(ErrorHandlingActions.loadData),
      switchMap((action) => this.backendService.throwException(action.value)),
      map((response) => ErrorHandlingActions.setData({ value: response })),
      catchSwitchMapError((error: any) => {
        return ErrorHandlingActions.setData({ value: undefined });
      })
    )
  );
}
export const catchSwitchMapError =
  (errorAction: (error: any) => any) =>
  <T>(source: Observable<T>) =>
    source.pipe(
      catchError((error, innerSource) =>
        innerSource.pipe(startWith(errorAction(error)))
      )
    );
