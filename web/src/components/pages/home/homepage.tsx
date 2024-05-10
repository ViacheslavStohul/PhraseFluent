import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import './homepage.scss';
import { NavLink } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';
import Select from 'react-select';
import * as langService from '../../../service/word.service';
import { callErrorToast } from '../../../store/slice/toast';
import { IOption } from '../../../interfaces/option';
import Card from '../../layouts/card/card';

const HomePage = () => {

  const user = useSelector(AuthSelectors.selectUser);
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [langs, setLangs] = useState<IOption[]>([]);

  useEffect(()=>{
    if (user){
      langService.getLangs()
      .then(languages=>{
        setLangs(languages.map(lang =>{
          return {
            value: lang.languageCode,
            label: lang.nativeName + '(' + lang.title + ')'
          }
        }));
      })
      .catch((error) => dispatch(callErrorToast({name: error.code, text: error.message})))
    }
  },[user, dispatch])

  const selectLanguage = (option?: string | number) => {
    if (!option){
      return;
    }
    console.log(option);
  }
  
  return (
    <>
    {
      user ?
      <></>
      :
    <Card>
    <div className='homepage'>
      <img className='home-image' src='image.jpg' alt='phrase-fluent'/>
      <h1>{t("homepage-greeting")}</h1>
      {t("homepage-text")}
      { user ?
      <Select
        classNamePrefix='select'
        className='select'
        aria-label='language'
        options={langs}
        onChange={(value) => selectLanguage(value?.value)}
        />
      :
      <NavLink to='/registration' end> 
        <button>{t("get-started")}</button>
      </NavLink>
      }
    </div>
    </Card>
    }
    </>
  );
}

export default HomePage;