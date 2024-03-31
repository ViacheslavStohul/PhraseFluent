import axios from 'axios';
import React, { FC, ReactNode, useEffect, useState } from 'react';
import { getToken } from '../../../functions/get-token';

interface AppProps {
  children: ReactNode;
}
const InterceptorManager: FC<AppProps> = (
  {children}) => {

  const [init, setInit] = useState(false);

  useEffect(() => {
    axios.interceptors.request.use((config) => {
      config.baseURL = 'https://localhost:44346';
      return config;
    });

    setInit(true);
  },[]);

  useEffect(() => {
    axios.interceptors.request.use((config) => {
      const token = getToken();
      if (token?.accessToken){
        config.headers.Authorization = `Bearer ${token.accessToken}`;
      }
      return config;
    });
  },[])

  return (
    <>
      {init && children}
    </>
  );
}

export default InterceptorManager;