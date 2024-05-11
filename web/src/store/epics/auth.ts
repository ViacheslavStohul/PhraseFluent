import { Epic, combineEpics, ofType } from "redux-observable";
import { mergeMap, of, switchMap } from "rxjs";
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

const RegisterSuccessEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.registerSuccess.type),
    mergeMap(() =>
      of(
        callToast({
          name: 'Successful registration',
          text: 'You registered successfully',
          type: ToastType.Success,
        }),
        AuthActions.getUserFetch()
      )
    )
);

const AuthSuccessEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.authSuccess.type),
    mergeMap(() =>
      of(
        callToast({name: 'Successful authorization', text: 'You authorized successfully', type: ToastType.Success}),
        AuthActions.getUserFetch()
      )
    )
);

const getUserFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.getUserFetch.type),
    switchMap(() =>
      AuthService.Get()
        .then((data) => {
          return AuthActions.getUserSuccess(data);
        })
        .catch((error) =>{ 
          localStorage.removeItem('token');
          window.location.reload();
          return callErrorToast({name: error.code, text: error.message})})
    )
);

const updateUserImageFetchEpic: Epic<any> = (action$) =>
  action$.pipe(
    ofType(AuthActions.updateUserImageFetch.type),
    switchMap((action) =>
      AuthService.updateImage(action.payload)
        .then(() => {
          return AuthActions.updateUserImageSuccess(action.payload);
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
  RefreshFetchEpic,
  RegisterSuccessEpic,
  updateUserImageFetchEpic,
  AuthSuccessEpic,
  getUserFetchEpic
);
