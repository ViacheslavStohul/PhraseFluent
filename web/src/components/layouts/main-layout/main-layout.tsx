import React from 'react';
import './main-layout.scss';
import Navbar from '../navbar/navbar';
import { Outlet } from 'react-router-dom';
function MainLayout() {
  return (
    <div className='app-body'>
      <Navbar/>
      <div className='main-body-body'>
        <Outlet/>
      </div>
      <footer>
        <div className="contact-info">
            <div className="contact-block">
                <strong>Адреса:</strong>
                <span>65065, м. Одеса, вул. Дворянська, 1/3, кім. 419, 418</span>
            </div>
            <div className="contact-block">
                <strong>Електронна почта:</strong>
                <a href="mailto:yurikkorn@gmail.com">yurikkorn@gmail.com</a>
            </div>
            <div className="contact-block">
                <strong>Контактні телефони:</strong>
                <span>Начальник відділу організації дистанційної роботи та навчання центру ІКТ</span>
                <span>Корнієнко Юрій Костянтинович</span>
                <span>(067) 934 99 41 (Viber, Telegram)</span>
                <span>(095) 934 70 89</span>
                <span>(093) 657 49 32</span>
            </div>
        </div>
    </footer>
    </div>
  );
}

export default MainLayout;
