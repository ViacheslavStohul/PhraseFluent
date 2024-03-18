import { Epic, combineEpics, ofType } from "redux-observable";
import { map, switchMap } from "rxjs";
import * as AuthService from "../../service/auth.service";
import { AuthActions } from "../slice/auth";
import { callErrorToast, callToast } from "../slice/toast";
import { ToastType } from "../../enum/toast";


const AuthFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.authFetch.type),
    switchMap((action) =>
      AuthService.Auth(action.payload)
        .then(({data}) => {
          return AuthActions.authSuccess({token: data, username:action.payload.username});
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
          return AuthActions.registerSuccess({token: data, username:action.payload.username});
        })
        .catch((error) => callErrorToast({name: error.code, text: error.message}))
    )
);

const RegisterSuccessEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.registerSuccess.type),
    map(() =>
      callToast({name: 'Successful registration', text: 'You registered successfully', type: ToastType.Success})
    ),
);

const AuthSuccessEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.authSuccess.type),
    map(() =>
      callToast({name: 'Successful authorization', text: 'You authorized successfully', type: ToastType.Success})
    ),
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
  RefreshFetchEpic,
  RegisterSuccessEpic,
  AuthSuccessEpic
);
