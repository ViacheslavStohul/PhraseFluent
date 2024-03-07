import axios from 'axios';
import React, { FC, ReactNode, useEffect, useState } from 'react';

interface AppProps {
  children: ReactNode;
}
const InterceptorManager: FC<AppProps> = (
  {children}) => {

  const [init, setInit] = useState(false);

  useEffect(() => {
    axios.interceptors.request.use((config) => {
      config.baseURL = 'https://localhost:7121';
      return config;
    });

    setInit(true);
  },[]);

  return (
    <>
      {init && children}
    </>
  );
}

export default InterceptorManager;