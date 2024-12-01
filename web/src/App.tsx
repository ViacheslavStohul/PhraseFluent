import React, { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import InterceptorManager from './components/layouts/interceptor-manager/interceptor-manager';
import ToastManager from './components/toast/toast-manager/toast-manager';
import "./i18n";
import { useTranslation } from 'react-i18next';

function App() {
  const { i18n } = useTranslation();

  useEffect(()=>{
    i18n.changeLanguage(navigator.language.split('-')[0]); 
  },[i18n]);
  

  return (
    <InterceptorManager>
      <Outlet/>
      <ToastManager/>
    </InterceptorManager>
  );
}

export default App;
