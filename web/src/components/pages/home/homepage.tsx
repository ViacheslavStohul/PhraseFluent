import React from 'react';
import { useTranslation } from 'react-i18next';
import './homepage.scss';
import { NavLink } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';
import Card from '../../layouts/card/card';
import TestList from '../../test-list/test-list';

const HomePage = () => {

  const user = useSelector(AuthSelectors.selectUser);
  const { t } = useTranslation();


  
  return (
    <>
    {
      user ?
      <TestList title={t('all-tests')}/>
      :
    <Card>
    <div className='homepage'>
      <img className='home-image' src='image.jpg' alt='phrase-fluent'/>
      <h1>{t("homepage-greeting")}</h1>
      {t("homepage-text")}
      <NavLink to='/registration' end> 
        <button>{t("get-started")}</button>
      </NavLink>
    </div>
    </Card>
    }
    </>
  );
}

export default HomePage;