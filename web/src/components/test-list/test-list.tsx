import React, { FC, useCallback, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import './test-list.scss';
import { IUser } from '../../interfaces/auth';
import { useDispatch } from 'react-redux';
import { callErrorToast } from '../../store/slice/toast';
import * as langService from '../../service/word.service';
import { InputFieldComponent } from '../fields/input-field/input-field';
import { debounce } from 'lodash';
import { Test } from '../../interfaces/test';
import Card from '../layouts/card/card';
import { useNavigate } from 'react-router-dom';
import PlusSVG from '../svg/plus';
import TestCard from './test-card/test-card';
import { useInView } from 'react-intersection-observer';
import { LangFieldComponent } from '../fields/lang-field/lang-field';

interface TestListProps {
  title: string;
  user?: IUser;
}

const TestList:FC<TestListProps> = ({title, user}) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [request, setRequest] = useState<langService.ListRequest>({Page: 1, Size: 20, Username: user?.username});
  const [tests, setTests] = useState<Test[]>([]);
  const navigate = useNavigate();
  const totalItems = useRef(0);
  const { ref, inView } = useInView({
    threshold: 0,
  });

  const selectLanguage = (option?: string) => {
    if (!option || option === request.Language){
      return;
    }
    setRequest((prev) => ({ ...prev, Language: option.split(' ')[0], Page: 1}));
  }

  useEffect(()=>{
    langService.getList(request).then(testList=>{
      if (request.Page === 1) {
        setTests(testList.items ?? []);
      } else {
        setTests((prev) => [...prev.slice(0, (request.Page - 1) * 10),...testList.items?? []]);
      }
      totalItems.current = testList.totalItems;
    })
    .catch((error) => dispatch(callErrorToast({name: error.code, text: error.message})))
  },[request,dispatch]);

  const handleSearch = useCallback((value: string) => {
    setRequest((prev) => ({ ...prev, Title: value, Page: 1 }));
  }, [setRequest]);

  useEffect(()=>{
    setRequest((prev)=> {
      if (inView && totalItems.current > prev.Page * prev.Size ) {
        return {
         ...prev,
          Page: prev.Page + 1
        }
      }
      return prev;
    });
  },[inView]);

  const handleChange = debounce(handleSearch, 500);
  
  return (
    <Card classes='card-column'>
      <div className='test-list-header'>
        <h1>{title}</h1>
        <button
          type='button'
          onClick = {()=>navigate('/new')}>
            <PlusSVG/>
          {t("create-test")}
        </button>
      </div>
      <div className='test-list-header'>
        <InputFieldComponent 
          labelText={t("search")}
          name='search'
          changed={handleChange}/>
        <div className='input-field'>
        <label
          className="label"
        >
          {t("language")}
        </label>
        <LangFieldComponent
          selectLanguage={(option) => selectLanguage(option.label)}/>
        </div>
      </div>
      <div className='test-table'>
      {
        tests.map(test => (
          <TestCard test={test} key={test.uuid}/>
        ))
      }
      <div ref={ref}>
      </div>
      </div>
    </Card>
  );
}

export default TestList;