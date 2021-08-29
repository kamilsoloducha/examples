import { Action } from '@ngrx/store';
import { RequestCancellationState } from './state';

export enum RequestCancellationActions{
    GET_DATE = '[REQUEST_CANCELLATION] GET_DATE',
    GET_DATE_SUCCESS = '[REQUEST_CANCELLATION] GET_DATE_SUCCESS',
    CANCEL_GET_DATE = '[REQUEST_CANCELLATION] CANCEL_GET_DATE',
}

export class GetDate implements Action {
    readonly type = RequestCancellationActions.GET_DATE;
    constructor() { }

    public static reduce(state: RequestCancellationState): RequestCancellationState {
        return {
            ...state,
        };
    }
}

export class CancelGetDate implements Action {
    readonly type = RequestCancellationActions.CANCEL_GET_DATE;
    constructor() { }

    public static reduce(state: RequestCancellationState): RequestCancellationState {
        return {
            ...state,
        };
    }
}

export class GetDateSuccess implements Action {
    readonly type = RequestCancellationActions.GET_DATE_SUCCESS;
    constructor(public value: string) { }

    public static reduce(state: RequestCancellationState, action: GetDateSuccess): RequestCancellationState {
        return {
            ...state,
            value: action.value
        };
    }
}

export type RequestCancellationType =
GetDate |
GetDateSuccess|
CancelGetDate;
