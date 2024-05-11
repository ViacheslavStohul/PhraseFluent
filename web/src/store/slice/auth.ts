import { createSlice } from "@reduxjs/toolkit";
import { AuthState, IAuth, IRegister, ITokenState, IUser } from "../../interfaces/auth";


const initialState: AuthState = {loading:false};

const AuthSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: (create) => ({
    registerFetch: create.reducer<IRegister>((state, _) => {
      state.loading = true;
    }),
    registerSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload));
    }),
    authFetch: create.reducer<IAuth>((state, _) => {
      state.loading = true;
    }),
    authSuccess: create.reducer<ITokenState>((state, action) => {
      state.loading = false;
      localStorage.setItem('token', JSON.stringify(action.payload));
    }),
    getUserFetch: create.reducer<void>((state, _) => {
      state.loading = true;
    }),
    getUserSuccess: create.reducer<IUser>((state, action) => {
      state.loading = false;
      state.user = action.payload;
    }),
    updateUserImageFetch: create.reducer<string>((state, _) => {
      state.loading = false;
    }),
    updateUserImageSuccess: create.reducer<string>((state, action) => {
      state.loading = false;
      if (state.user)
        state.user.imageUrl = action.payload;
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
      state.user = undefined;
      localStorage.removeItem('token');
    }),
  }),
  selectors: {
    selectAuthLoading: (state) => state.loading,
    selectUser: (state) => state.user
  }
});

const AuthSelectors = AuthSlice.selectors;
const AuthActions = AuthSlice.actions;

type AuthUnionType = ReturnType<
  | typeof AuthActions.registerFetch
  | typeof AuthActions.registerSuccess
  | typeof AuthActions.authFetch
  | typeof AuthActions.authSuccess
  | typeof AuthActions.getUserFetch
  | typeof AuthActions.getUserSuccess
  | typeof AuthActions.updateUserImageFetch
  | typeof AuthActions.updateUserImageSuccess
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