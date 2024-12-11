import React, { Fragment, useEffect, useState } from 'react';
import { useDispatch} from 'react-redux';
import { useNavigate, useSearchParams } from 'react-router-dom';
import * as testService from '../../../service/test.service';
import { callErrorToast } from '../../../store/slice/toast';
import './test.scss';
import Card from '../../layouts/card/card';
import { BeginTestResponse, Test } from '../../../interfaces/test';
import Checkbox from '../../fields/checkbox/checkbox';
import { TextareaFieldComponent } from '../../fields/textarea/textarea';
import { InputFieldComponent } from '../../fields/input-field/input-field';

const TestPage = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [searchParams] = useSearchParams();
  const [test, setTest] = useState<BeginTestResponse>();
  const [text, setText] = useState<string | undefined>();
  const [info, setInfo] = useState<Test>();

  useEffect(() => {
    let id = searchParams.get('id');
    if (id) {
      testService.getTest(id).then((test)=>{
        setInfo(test);
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[navigate, searchParams, dispatch]);


  const start = () => {
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
  };

  const changeOption = (uuid: string) => {
    setTest(prev => {
      if (!prev || !prev.card){
        return prev;
      }
      return {
       ...prev,
        card: {
         ...prev.card,
          answerOptions: prev.card.answerOptions?.map(option => {
            if (option.uuid === uuid) {
              if (option.isCorrect && option.isAllowedText){
                setText(undefined);
              }
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
      answerString: text,
      pickedOptions: test?.card?.answerOptions?.filter(option => option.isCorrect).map(option => option.uuid??'') ?? undefined
    }).then((res)=>{
      window.scrollTo(0, 0);
      setTest(res);
      setText('');
    }).catch((error) => {
      dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
    });
  }

  return (
    <Card classes='testing-card'>
      { !test ?
        info && 
        <>
          <h2>{info.title}</h2>
          <p>{info.description}</p>
          <div className='right'>
            <button onClick={start}>Розпочати</button>
          </div>
        </>
      :
      test?.card ?
      <>
      <div className='counter'>{test?.currentQuestion}/{test?.questions}</div>
      <h3>{test?.card?.question}</h3>
      { test?.card.questionType === 'TestOneAnswer' &&
      <p className='no-indent'>
        Оберіть одну з відповідей:
        </p>
      }
      { test?.card.questionType === 'TestManyAnswers' &&
      <p className='no-indent'>
        Оберіть одну або декілька відповідей:
      </p>
      }
      { test?.card?.questionType === 'Text' ?
        <TextareaFieldComponent
          labelText='Відповідь'
          name='answer'
          value={text}
          changed={setText}
        />
        :
        <div className='answer-grid'>
        {
          test && test.card?.answerOptions?.map((option)=> (
            <Fragment key={option.uuid}>
            <Checkbox
              label={option.optionText}
              checked={option.isCorrect} 
              onChange={() => changeOption(option.uuid??'')} 
              isRadio={test?.card?.questionType === 'TestOneAnswer'}
              />
              {
                option.isAllowedText &&
                <InputFieldComponent
                labelText=''
                name='answer'
                value={text ?? ''}
                maxLength={40}
                changed={setText}
                disabled={!option.isCorrect}
              />
              }
            </Fragment>
          ))
        }
      </div>
      }
      <div className='right'>
        <button onClick={submit} disabled={(test?.card.questionType !== 'Text' && (!test?.card?.answerOptions || test?.card?.answerOptions.every(option => !option.isCorrect)))}>Наступне питання</button>
      </div>
      </>
      :
      <>
      <h2>Дякуємо за проходження опитування!</h2>
      <p className='no-indent'>Ваші результати були надіслані.</p>
      </>
      }
    </Card>
  );
}

export default TestPage;