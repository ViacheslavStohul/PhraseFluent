import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';
import { useNavigate, useSearchParams } from 'react-router-dom';
import * as testService from '../../../service/word.service';
import { callErrorToast } from '../../../store/slice/toast';
import './test.scss';
import Card from '../../layouts/card/card';
import { BeginTestResponse } from '../../../interfaces/test';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useTranslation } from 'react-i18next';
import OptionCard from './option/option';

const User = () => {
  const user = useSelector(AuthSelectors.selectUser);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const {t} = useTranslation();
  const [searchParams] = useSearchParams();
  const [test, setTest] = useState<BeginTestResponse>();
  const [text, setText] = useState<string>('');

  useEffect(() => {
    if (!user){
      navigate('/');
    }

    let id = searchParams.get('id');
    if (id) {
      testService.beginTest(id).then((res)=>{
        setTest(res);
        setText('');
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[user, navigate, searchParams, dispatch]);

  const changeOption = (uuid: string) => {
    setTest(prev => {
      if (!prev || !prev.card){
        return prev;
      }
      return {
       ...prev,
        card: {
         ...prev.card,
          answerOptions: prev.card.answerOptions.map(option => {
            if (option.uuid === uuid) {
              return {
               ...option,
                isCorrect:!option.isCorrect
              }
            }
            return prev?.card?.questionType === 'TestOneAnswer'? {...option, isCorrect:false}: option;
          })
        }
      }
    });
  }
  
  const submit = () => {
    testService.nextTest({
      cardUuid: test?.card?.uuid ?? '',
      testAttemptUuid: test?.testAttemptUuid ?? '',
      answerString: text? text : undefined,
      pickedOptions: test?.card?.answerOptions.filter(option => option.isCorrect).map(option => option.uuid??'') ?? undefined
    }).then((res)=>{
      setTest(res);
      setText('');
    }).catch((error) => {
      dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
    });
  }

  const getText = () => {
    if (!test?.card){
      return '';
    }
    if (test?.card.questionType === 'Text'){
      return t('enter-answer');
    } else if (test?.card.questionType === 'TestOneAnswer'){
      return t('choose-answer');
    } else if (test?.card.questionType === 'TestManyAnswers'){
      return t('choose-answers');
    }
  }

  return (
    <Card classes='testing-card'>
      { test?.card &&
      <>
      <div className='counter'>{test?.currentQuestion}/{test?.questions}</div>
      <h2>{test?.card?.question}</h2>
      <p style={{textAlign:"center"}}>{getText()}</p>
      { test?.card?.questionType === 'Text' ?
        <InputFieldComponent
          labelText={t('answer-text')}
          name='answer'
          value={text}
          changed={setText}
        />
        :
        <div className='answer-grid'>
        {
          test && test.card?.answerOptions.map((option, index)=> (
            <OptionCard option={option} emit={() => changeOption(option.uuid??'')} key={index}/>
          ))
        }
      </div>
      }
      <div className='right'>
        <button onClick={submit}>Next</button>
      </div>
      </>
      }
    </Card>
  );
}

export default User;