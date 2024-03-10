import axios, { AxiosResponse } from "axios";
import { IAuth, IRegister, ITokenState } from "../components/interfaces/auth";

const baseUrl = '/api/auth'

export const Auth = async (auth :IAuth): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/token`,auth);

  export const Register = async (register :IRegister): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/register`,register);

  export const Refresh = async (token: ITokenState): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/token/refresh`,token);