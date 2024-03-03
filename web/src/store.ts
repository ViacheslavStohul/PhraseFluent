import { configureStore, Tuple } from "@reduxjs/toolkit";
import { combineReducers } from "redux";
import { combineEpics, createEpicMiddleware } from "redux-observable";

const epicMiddleware = createEpicMiddleware();
export const middleware = [epicMiddleware];

export const rootReducers = combineReducers({
});

export const store = configureStore({
  reducer: rootReducers,
  middleware: () => new Tuple(epicMiddleware)
});

epicMiddleware.run(combineEpics());

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
