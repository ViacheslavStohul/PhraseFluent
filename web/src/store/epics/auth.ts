import { Epic, combineEpics, ofType } from "redux-observable";
import { switchMap } from "rxjs";
import * as AuthService from "../../service/auth.service";
import { AuthActions } from "../slice/auth";
import { callErrorToast } from "../slice/toast";


const AuthFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.authFetch.type),
    switchMap((action) =>
      AuthService.Auth(action.payload)
        .then(({data}) => {
          return AuthActions.authSuccess(data);
        })
        .catch((error) => callErrorToast({name: error.code, text: error.message}))
    )
);

const RegisterFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.registerFetch.type),
    switchMap((action) =>
      AuthService.Register(action.payload)
        .then(({data}) => {
          return AuthActions.registerSuccess(data);
        })
        .catch((error) => callErrorToast({name: error.code, text: error.message}))
    )
);

const RefreshFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.refreshFetch.type),
    switchMap((action) =>
      AuthService.Refresh(action.payload)
        .then(({data}) => {
          return AuthActions.refreshSuccess(data);
        })
        .catch((error) => callErrorToast({name: error.code, text: error.message}))
    )
);

export const authEpics = combineEpics<any>(
  AuthFetchEpic,
  RegisterFetchEpic,
  RefreshFetchEpic
);
