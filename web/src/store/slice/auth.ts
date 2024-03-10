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
    registerSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresIn = action.payload.expiresIn;
    }),
    authFetch: create.reducer<IAuth>((state, _) => {
      state.loading = true;
    }),
    authSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresIn = action.payload.expiresIn;
    }),
    refreshFetch: create.reducer<ITokenState>((state, _) => {
      state.loading = true;
    }),
    refreshSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      state.accessToken = action.payload.accessToken;
      state.refreshToken = action.payload.refreshToken;
      state.expiresIn = action.payload.expiresIn;
    }),
  }),
  selectors: {
    selectAuthLoading: (state) => state.loading,
    selectToken: (state) => state.accessToken,
    selectRefreshToken: (state) => state.refreshToken,
    selectExpiresIn: (state) => state.expiresIn,
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
>;

export {
  AuthSlice,
  AuthSelectors,
  AuthActions
};
export type { AuthUnionType };