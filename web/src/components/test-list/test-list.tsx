import React, { FC, useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import './test-list.scss';
import Select from 'react-select';
import { IUser } from '../../interfaces/auth';
import { useDispatch } from 'react-redux';
import { IOption } from '../../interfaces/option';
import { callErrorToast } from '../../store/slice/toast';
import * as langService from '../../service/word.service';
import { InputFieldComponent } from '../fields/input-field/input-field';
import { debounce } from 'lodash';
import { Test } from '../../interfaces/test';

interface TestListProps {
  user?: IUser;
}

const TestList:FC<TestListProps> = ({user}) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [langs, setLangs] = useState<IOption[]>([]);
  const [request, setRequest] = useState<langService.ListRequest>({Page: 1, Size: 20, Username: user?.username});
  const [tests, setTests] = useState<Test[]>([]);

  useEffect(()=>{
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
  },[dispatch])

  const selectLanguage = (option?: string | number) => {
    if (!option){
      return;
    }
    setRequest((prev) => ({ ...prev, Language: option as string }));
  }

  useEffect(()=>{
    langService.getList(request).then(tests=>{
      setTests(tests);
    })
    .catch((error) => dispatch(callErrorToast({name: error.code, text: error.message})))
  },[request,dispatch]);

  const handleSearch = useCallback((value: string) => {
    setRequest((prev) => ({ ...prev, search: value }));
  }, [setRequest]);

  const handleChange = debounce(handleSearch, 500);
  
  return (
    <>
     <InputFieldComponent 
      labelText={t("search")}
      name='search'
      changed={handleChange}/>
    <Select
      classNamePrefix='select'
      className='select'
      aria-label='language'
      options={langs}
      onChange={(value) => selectLanguage(value?.value)}/>
      {
        tests.map(test => (
          <div key={test.title}>
            <h2>{test.title}</h2>
            <p>{test.description}</p>
          </div>
        ))
      }
    </>
  );
}

export default TestList;