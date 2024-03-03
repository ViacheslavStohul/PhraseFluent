import React from 'react';
import './main-layout.scss';
import Navbar from '../../navbar/navbar';
import { Outlet } from 'react-router-dom';
function MainLayout() {
  return (
    <div className='app-body'>
      <Navbar/>
      <div className='main-body-body'>
        <Outlet/>
      </div>
    </div>
  );
}

export default MainLayout;
