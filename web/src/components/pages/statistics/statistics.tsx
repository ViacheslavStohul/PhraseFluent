import React, { useEffect, useState } from 'react';
import './statistics.scss';
import { callErrorToast } from '../../../store/slice/toast';
import * as testService from '../../../service/test.service';
import { useDispatch } from 'react-redux';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Card from '../../layouts/card/card';
import { Test } from '../../../interfaces/test';
import QuestionStats from './question-stats/question-stats';

const Statistics = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [searchParams] = useSearchParams();
  const [test, setTest] = useState<Test>();
  const [answerOption, setAnswerOption] = useState<string>();

  useEffect(() => {
    let id = searchParams.get('id');
    if (id) {
      testService.statsTest(id, answerOption).then((res)=>{
        setTest(res);
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[navigate, searchParams, dispatch, answerOption]);

  const download = () => {
    let id = searchParams.get('id');
    if (id) {
      testService.statsExcel(id).then(()=>{}).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    }
  };

  return (
    <div className='statistics'>
    {
      test &&
      <>
        <Card classes='statistics-card'>
          <h2>{test.title}</h2>
          <p>{test.description}</p>
          <span>Кількість опитуваних: {test.completedAttempts}</span>
          <button onClick={download}>Завантажити</button>
        </Card>
        {
        test.cards.map(card => 
          <QuestionStats question={card} key={card.uuid} selected={answerOption} check={(uuid) => setAnswerOption(prev => prev === uuid ? undefined : uuid)}/>
        )
        }
      </>
    }
    </div>
  );
}

export default Statistics;