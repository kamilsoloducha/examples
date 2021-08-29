import { CancelGetDate, GetDate, GetDateSuccess, RequestCancellationActions, RequestCancellationType } from './actions';
import { initState, RequestCancellationState } from './state';

export function reducer(state = initState, action: RequestCancellationType): RequestCancellationState{
    switch (action.type){
        case RequestCancellationActions.GET_DATE: return GetDate.reduce(state);
        case RequestCancellationActions.GET_DATE_SUCCESS: return GetDateSuccess.reduce(state, action);
        case RequestCancellationActions.CANCEL_GET_DATE: return CancelGetDate.reduce(state);
        default: return state;
    }
}
