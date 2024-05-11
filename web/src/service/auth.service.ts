import axios, { AxiosResponse } from "axios";
import { IAuth, ITokenState, IRegister, IUser } from "../interfaces/auth";

const baseUrl = '/api/auth'

export const Auth = async (auth :IAuth): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/token`,auth);

  export const Register = async (register :IRegister): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/register`,register);

  export const Refresh = async (token: ITokenState): Promise<AxiosResponse<ITokenState>> => 
  axios.post(`${baseUrl}/token/refresh`,token);

  export const Get = async (uuid?:string): Promise<IUser> => {
    const {data} = await axios.get(`/get${uuid ? '?uuid='+uuid : ''}`);
    return data;
  }

  export const updateImage = async (link: string): Promise<void> => 
    axios.put(`/img?imageUrl=${link}`);
