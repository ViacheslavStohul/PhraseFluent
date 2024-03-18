export interface IAuth {
  username: string;
  password: string;
}

export interface IRegister extends IAuth {
  repeatedPassword: string;
}

export interface ITokenState {
  accessToken?: string;
  refreshToken?: string;
  expiresIn?: number;
}

export interface TokenState extends ITokenState {
  loading: boolean;
  username?: string;
}