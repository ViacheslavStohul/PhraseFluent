import axios from 'axios';
import React, { FC, ReactNode, useEffect, useState } from 'react';
import { getToken } from '../../../functions/get-token';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';

interface AppProps {
  children: ReactNode;
}

enum InitStates {
  Not = 0,
  Init = 1,
  FullInit = 2,
}
const InterceptorManager: FC<AppProps> = (
  {children}) => {

  const [init, setInit] = useState<InitStates>(InitStates.Init);
  const user = useSelector(AuthSelectors.selectUser);

  const dispatch = useDispatch();

  useEffect(() => {
    axios.interceptors.request.use((config) => {
      config.baseURL = 'https://localhost:44346';
      return config;
    });

    axios.interceptors.request.use((config) => {
      const token = getToken();
      if (token?.accessToken){
        config.headers.Authorization = `Bearer ${token.accessToken}`;
      }
      return config;
    });

    setInit(InitStates.Init);

  },[]);

  useEffect(()=>{
    if (init === InitStates.Init){
      const token = getToken();
      if (token){
        dispatch(AuthActions.getUserFetch());
      } else {
        setInit(InitStates.FullInit);
      }
    }
  },[dispatch, init]);

  useEffect(() => {
    if (user) {
      setInit(InitStates.FullInit);
    }
  }, [user]);

  return (
    <>
      {init === InitStates.FullInit && children}
    </>
  );
}

export default InterceptorManager;