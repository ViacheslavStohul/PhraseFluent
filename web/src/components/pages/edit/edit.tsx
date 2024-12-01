import React, { useState } from 'react';
import './edit.scss';
import '../new/new.scss';
import { ICard, Test } from '../../../interfaces/test';
import Card from '../../layouts/card/card';
import CreateQuestionCard from './create-question-card/create-question-card';
import { useTranslation } from 'react-i18next';
import AnswerCard from './answer-card/answer-card';
import { useNavigate } from 'react-router-dom';

const Edit = ({test}:{test: Test}) => {
  const { t } = useTranslation();
  const [imageError, setImageError] = useState(false);
  const [cards, setCards]=useState<ICard[]>([]);
  const navigate = useNavigate();

  const handleError = (): void => {
   setImageError(true);
  };

  const addQuestion = (question: ICard): void => {
    setCards([...cards, question]);
  }

  
  return (
    <div className='new-edit'>
      <Card classes='new-test'>
        <div className='new-test-text'>
          <h2>{test.title}</h2>
          <label>{test.language.nativeName+'('+test.language.title+')'}</label>
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
            <h2>{t('question')} {index+1}</h2>
            {card.question}
            {card.questionType==='Text' ?
              <label>{t('correct-answer')}: {card?.answerOptions?.[0]?.optionText}</label>
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
      <button onClick={()=>navigate('/')}>{t('finish-test-creation')}</button>
    </div>
  );
}

export default Edit;