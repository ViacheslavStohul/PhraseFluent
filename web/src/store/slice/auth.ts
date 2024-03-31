import { createSlice } from "@reduxjs/toolkit";
import { IAuth, IRegister, ITokenState, AuthState } from "../../components/interfaces/auth";


const initialState: AuthState = {loading:false};

const AuthSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: (create) => ({
    registerFetch: create.reducer<IRegister>((state, _) => {
      state.loading = true;
    }),
    registerSuccess: create.reducer<{token:ITokenState, username: string}>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload.token));
      state.username = action.payload.username;
    }),
    authFetch: create.reducer<IAuth>((state, _) => {
      state.loading = true;
    }),
    authSuccess: create.reducer<{token:ITokenState, username: string}>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload.token));
      state.username = action.payload.username;
    }),
    refreshFetch: create.reducer<ITokenState>((state, _) => {
      state.loading = true;
    }),
    refreshSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload));
    }),
    logout: create.reducer<void>((state) => {
      state.loading = false;
      state.username = undefined;
      localStorage.removeItem('token');
    }),
  }),
  selectors: {
    selectAuthLoading: (state) => state.loading,
    selectUsername: (state) => state.username
  }
});

const AuthSelectors = AuthSlice.selectors;
const AuthActions = AuthSlice.actions;

type AuthUnionType = ReturnType<
  | typeof AuthActions.registerFetch
  | typeof AuthActions.registerSuccess
  | typeof AuthActions.authFetch
  | typeof AuthActions.authSuccess
  | typeof AuthActions.refreshFetch
  | typeof AuthActions.refreshSuccess
  | typeof AuthActions.logout
>;

export {
  AuthSlice,
  AuthSelectors,
  AuthActions
};
export type { AuthUnionType };