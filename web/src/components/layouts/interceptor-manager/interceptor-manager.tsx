import axios from 'axios';
import React, { FC, ReactNode, useEffect, useState } from 'react';
import { AuthSelectors } from '../../../store/slice/auth';
import { useSelector } from 'react-redux';

interface AppProps {
  children: ReactNode;
}
const InterceptorManager: FC<AppProps> = (
  {children}) => {

  const [init, setInit] = useState(false);
  const token = useSelector(AuthSelectors.selectToken);

  useEffect(() => {
    axios.interceptors.request.use((config) => {
      config.baseURL = 'https://localhost:44346';
      if (token){
        config.headers.Authorization = `Bearer ${token}`;
        }
      return config;
    });

    setInit(true);
  },[token]);

  return (
    <>
      {init && children}
    </>
  );
}

export default InterceptorManager;