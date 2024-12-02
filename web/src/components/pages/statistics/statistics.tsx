import React, { useEffect } from 'react';
import './statistics.scss';
import { callErrorToast } from '../../../store/slice/toast';
import * as testService from '../../../service/word.service';
import { useDispatch } from 'react-redux';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Card from '../../layouts/card/card';

const Statistics = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    let id = searchParams.get('id');
    if (id) {
      testService.statsTest(id).then((res)=>{
        console.log(res);
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[navigate, searchParams, dispatch]);

  return (
    <Card>
      <span></span>
    </Card>
  );
}

export default Statistics;