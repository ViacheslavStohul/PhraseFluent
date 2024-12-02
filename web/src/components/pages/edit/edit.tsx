import React, { useEffect, useState } from 'react';
import './edit.scss';
import '../new/new.scss';
import { ICard, Test } from '../../../interfaces/test';
import Card from '../../layouts/card/card';
import CreateQuestionCard from './create-question-card/create-question-card';
import AnswerCard from './answer-card/answer-card';
import { useNavigate, useSearchParams } from 'react-router-dom';
import * as testService from '../../../service/word.service';
import { useDispatch } from 'react-redux';
import { callErrorToast } from '../../../store/slice/toast';
import { Protection } from '../../protection/protection';

const Edit = () => {
  const [imageError, setImageError] = useState(false);
  const [cards, setCards]=useState<ICard[]>([]);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [test, setTest]=useState<Test>();
  const dispatch = useDispatch();

  useEffect(() => {
    let id = searchParams.get('id');
    if (id) {
      testService.getTest(id).then((test)=>{
        setTest(test);
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[navigate, searchParams, dispatch]);

  const handleError = (): void => {
   setImageError(true);
  };

  const addQuestion = (question: ICard): void => {
    setCards([...cards, question]);
  }

  
  return (
    <Protection>
    { test &&
    <div className='new-edit'>
      <Card classes='new-test'>
        <div className='new-test-text'>
          <h2>{test.title}</h2>
          <p>{test.description}</p>
        </div>
        <img 
        alt={test?.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': test?.imageUrl?? ''}
        onError={handleError}/>
      </Card>
      {
        cards.map((card, index) => (
          <Card classes='new-card' key={index+'-question'}>
            <h2>Питання {index+1}</h2>
            {card.question}
            {card.questionType==='Text' ?
              <></>
              :
              <div className='answer-grid'>
                {
                  card.answerOptions && card.answerOptions.map((option, index)=> (
                    <AnswerCard option={option} key={index}/>
                  ))
                }
              </div>
            }
          </Card>
        ))
      }
      <CreateQuestionCard emit={addQuestion} testId={test.uuid}/>
      <button onClick={()=>navigate('/')}>Завершити</button>
    </div>
    }
    </Protection>
  );
}

export default Edit;