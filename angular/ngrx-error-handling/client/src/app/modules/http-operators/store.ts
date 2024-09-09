import { state } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  createActionGroup,
  createFeature,
  createFeatureSelector,
  createReducer,
  createSelector,
  on,
  props,
} from '@ngrx/store';
import { concatMap, exhaustMap, map, mergeMap, switchMap } from 'rxjs';

export type HttpOperatorsState = {
  isLoading: boolean;
  values: string[];
};

export const initState: HttpOperatorsState = {
  values: [],
  isLoading: false,
};

export const HttpOperatorsActions = createActionGroup({
  source: 'httpOperators',
  events: {
    loadDataSwitchMap: props<{ timeout: number }>(),
    loadDataConcatMap: props<{ timeout: number }>(),
    loadDataMergeMap: props<{ timeout: number }>(),
    loadDataExhaustMap: props<{ timeout: number }>(),
    loadDataMap: props<{ timeout: number }>(),
    setValue: props<{ value: string }>(),
  },
});

export const HttpOperatorsFetaure = createFeature({
  name: 'httpOperators',
  reducer: createReducer(
    initState,
    on(HttpOperatorsActions.loadDataSwitchMap, (state) => {
      return { ...state, isLoading: true };
    }),
    on(HttpOperatorsActions.loadDataConcatMap, (state) => {
      return { ...state, isLoading: true };
    }),
    on(HttpOperatorsActions.loadDataMergeMap, (state) => {
      return { ...state, isLoading: true };
    }),
    on(HttpOperatorsActions.loadDataExhaustMap, (state) => {
      return { ...state, isLoading: true };
    }),
    on(HttpOperatorsActions.loadDataMap, (state) => {
      return { ...state, isLoading: true };
    }),
    on(HttpOperatorsActions.setValue, (state, action) => {
      return {
        ...state,
        isLoading: false,
        values: [...state.values, action.value],
      };
    })
  ),
});

export class HttpOperatorsEffects {
  actions$ = inject(Actions);
  backendService = inject(HttpClient);

  switchMap$ = createEffect(() =>
    this.actions$.pipe(
      ofType(HttpOperatorsActions.loadDataSwitchMap),
      switchMap((action) =>
        this.backendService.get<string>(
          `http://localhost:5000/value/${action.timeout}`
        )
      ),
      map((response) => HttpOperatorsActions.setValue({ value: response }))
    )
  );

  concatMap$ = createEffect(() =>
    this.actions$.pipe(
      ofType(HttpOperatorsActions.loadDataConcatMap),
      concatMap((action) =>
        this.backendService.get<string>(
          `http://localhost:5000/value/${action.timeout}`
        )
      ),
      map((response) => HttpOperatorsActions.setValue({ value: response }))
    )
  );

  mergeMap$ = createEffect(() =>
    this.actions$.pipe(
      ofType(HttpOperatorsActions.loadDataMergeMap),
      mergeMap((action) =>
        this.backendService.get<string>(
          `http://localhost:5000/value/${action.timeout}`
        )
      ),
      map((response) => HttpOperatorsActions.setValue({ value: response }))
    )
  );

  exchaustMap$ = createEffect(() =>
    this.actions$.pipe(
      ofType(HttpOperatorsActions.loadDataExhaustMap),
      exhaustMap((action) =>
        this.backendService.get<string>(
          `http://localhost:5000/value/${action.timeout}`
        )
      ),
      map((response) => HttpOperatorsActions.setValue({ value: response }))
    )
  );

  //   map$ = createEffect(() =>
  //     this.actions$.pipe(
  //       ofType(HttpOperatorsActions.loadDataMap),
  //       map((action) =>
  //         this.backendService.get<string>(
  //           `http://localhost:5000/value/${action.timeout}`
  //         )
  //       ),
  //       map((response) => HttpOperatorsActions.setValue({ value: response }))
  //     )
  //   );
}

const selectState = createFeatureSelector<HttpOperatorsState>('httpOperators');

export const selectValues = createSelector(
  selectState,
  (state) => state.values
);
