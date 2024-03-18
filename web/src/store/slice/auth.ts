import { createSlice } from "@reduxjs/toolkit";
import { IAuth, IRegister, ITokenState, TokenState } from "../../components/interfaces/auth";

const getToken = () => {
  const token = localStorage.getItem('token');

  if (token) {
    return {...JSON.parse(token), loading: false};
  }
  else {
    return {loading: false};
  }
}

const initialState: TokenState = getToken();

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
      state.accessToken = action.payload.token.accessToken;
      state.refreshToken = action.payload.token.refreshToken;
      state.expiresIn = action.payload.token.expiresIn;
      state.username = action.payload.username;
    }),
    authFetch: create.reducer<IAuth>((state, _) => {
      state.loading = true;
    }),
    authSuccess: create.reducer<{token:ITokenState, username: string}>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload.token));
      state.accessToken = action.payload.token.accessToken;
      state.refreshToken = action.payload.token.refreshToken;
      state.expiresIn = action.payload.token.expiresIn;
      state.username = action.payload.username;
    }),
    refreshFetch: create.reducer<ITokenState>((state, _) => {
      state.loading = true;
    }),
    refreshSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload));
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresIn = action.payload.expiresIn;
    }),
    logout: create.reducer<void>((state) => {
      state.loading = false;
      state.accessToken = undefined;
      state.refreshToken = undefined;
      state.expiresIn = undefined;
      state.username = undefined;
      localStorage.removeItem('token');
    }),
  }),
  selectors: {
    selectAuthLoading: (state) => state.loading,
    selectToken: (state) => state.accessToken,
    selectRefreshToken: (state) => state.refreshToken,
    selectExpiresIn: (state) => state.expiresIn,
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