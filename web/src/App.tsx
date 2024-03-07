import React from 'react';
import { Outlet } from 'react-router-dom';
import InterceptorManager from './components/layouts/interceptor-manager/interceptor-manager';

function App() {
  return (
    <InterceptorManager>
      <Outlet/>
    </InterceptorManager>
  );
}

export default App;
