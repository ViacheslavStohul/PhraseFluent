import axios, { AxiosResponse } from "axios";


export const Auth = async (): Promise<
  AxiosResponse<any>
> => axios.post<any>(`/token`,{});