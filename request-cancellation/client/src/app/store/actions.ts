import { Action } from '@ngrx/store';

export enum RootTypes {
    RequestFailed = '[ROOT] REQUEST_FAILED',

    Navigate = '[ROOT] NAVIGATE'
}

export class RequestFailed implements Action {
    public readonly type = RootTypes.RequestFailed;

    constructor(public payload: { error: any }) { }
}

export class Navigate implements Action {
    public readonly type = RootTypes.Navigate;

    constructor(public payload: { command: any[] }) { }
}

export type RootActions = RequestFailed |
    Navigate;
