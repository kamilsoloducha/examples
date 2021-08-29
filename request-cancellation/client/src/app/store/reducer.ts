import { RootActions } from './actions';
import { initRootState, RootState } from './state';


export function rootReducer(state = initRootState, action: RootActions): RootState {
    switch (action.type) {
        default: return state;
    }
}
