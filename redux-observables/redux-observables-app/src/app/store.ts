import { configureStore, ThunkAction, Action } from "@reduxjs/toolkit";
import { combineEpics, createEpicMiddleware } from "redux-observable";
import counterReducer, { pingEpic, pongEpic } from "../features/counter/counterSlice";

export const rootEpic = combineEpics(pingEpic, pongEpic);
const epicMiddleware = createEpicMiddleware();

export const store = configureStore({
  reducer: {
    counter: counterReducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(epicMiddleware),
});
epicMiddleware.run(rootEpic);

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
