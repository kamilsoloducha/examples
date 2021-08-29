import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as actions from './actions';
import {map, switchMap, takeUntil} from 'rxjs/operators';

@Injectable()
export class RequestCancellationEffects{

    constructor(private readonly actions$: Actions,
                private readonly http: HttpClient)
                { }

    getDate$ = createEffect(() => {
        return this.actions$.pipe(
            ofType<actions.GetDate>(actions.RequestCancellationActions.GET_DATE),
            switchMap(() => this.http.get<string>('http://localhost:5000/date/ct')),
            map(value => new actions.GetDateSuccess(value)),
            takeUntil(this.actions$.pipe(ofType<actions.CancelGetDate>(actions.RequestCancellationActions.CANCEL_GET_DATE)))
        );
    });
}
