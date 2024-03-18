import { configureStore, Tuple } from "@reduxjs/toolkit";
import { combineReducers } from "redux";
import { combineEpics, createEpicMiddleware } from "redux-observable";
import { AuthSlice } from "./store/slice/auth";
import { authEpics } from "./store/epics/auth";
import { toastSlice } from "./store/slice/toast";

const epicMiddleware = createEpicMiddleware();
export const middleware = [epicMiddleware];

export const rootReducers = combineReducers({
  auth: AuthSlice.reducer,
  toasts: toastSlice.reducer
});

export const store = configureStore({
  reducer: rootReducers,
  middleware: () => new Tuple(epicMiddleware)
});

epicMiddleware.run(combineEpics(authEpics));

export type RootState = ReturnType<typeof store.getState>;
