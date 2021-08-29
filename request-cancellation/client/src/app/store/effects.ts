import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { tap } from 'rxjs/operators';
import { Navigate, RequestFailed, RootTypes } from './actions';

@Injectable()
export class RootEffects {

    constructor(private readonly actions$: Actions,
                private readonly router: Router) { }

}
