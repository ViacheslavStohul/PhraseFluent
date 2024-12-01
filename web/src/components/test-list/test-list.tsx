import React, { FC, useCallback, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import './test-list.scss';
import { useDispatch, useSelector } from 'react-redux';
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
import { AuthSelectors } from '../../store/slice/auth';

interface TestListProps {
  title: string;
}

const TestList:FC<TestListProps> = ({title}) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const user = useSelector(AuthSelectors.selectUser);
  const [request, setRequest] = useState<langService.ListRequest>({Page: 1, Size: 20});
  const [tests, setTests] = useState<Test[]>([]);
  const navigate = useNavigate();
  const totalItems = useRef(0);
  const { ref, inView } = useInView({
    threshold: 0,
  });

  useEffect(()=>{
    langService.getList(request).then(testList=>{
      if (request.Page === 1) {
        setTests(testList.items ?? []);
      } else {
        setTests((prev) => [...prev.slice(0, (request.Page - 1) * request.Size),...testList.items?? []]);
      }
      totalItems.current = testList.totalItems;
    })
    .catch((error) => dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.message})))
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
        { user &&
        <button
          type='button'
          onClick = {()=>navigate('/new')}>
            <PlusSVG/>
          Створити тест
        </button>
        }
      </div>
      <div className='test-list-header'>
        <InputFieldComponent 
          labelText={t("search")}
          name='search'
          changed={handleChange}/>
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