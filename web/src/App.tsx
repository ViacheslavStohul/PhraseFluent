import React from 'react';
import { Outlet } from 'react-router-dom';
import InterceptorManager from './components/layouts/interceptor-manager/interceptor-manager';
import { ToastProvider } from './context/toasts';

function App() {
  return (
    <InterceptorManager>
      <ToastProvider>
      <Outlet/>
      </ToastProvider>
    </InterceptorManager>
  );
}

export default App;
