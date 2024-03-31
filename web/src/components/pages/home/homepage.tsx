import React from 'react';
import { useTranslation } from 'react-i18next';
import './homepage.scss';

const HomePage = () => {

  const { t } = useTranslation();
  
  return (
    <div className='homepage'>
      <img className='home-image' src='image.png' alt='phrase-fluent'/>
      {t("homepage")}</div>
  );
}

export default HomePage;