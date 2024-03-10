import React from 'react';
import { Outlet } from 'react-router-dom';
import InterceptorManager from './components/layouts/interceptor-manager/interceptor-manager';
import ToastManager from './context/toast/toast-manager/toast-manager';

function App() {
  return (
    <InterceptorManager>
        <Outlet/>
      <ToastManager/>
    </InterceptorManager>
  );
}

export default App;
