import React, { useState } from 'react';
import { ICard} from '../../../../interfaces/test';
import './question-stats.scss';
import Card from '../../../layouts/card/card';

interface IProps {
  question: ICard;
  check: (id: string) => void;
}


const QuestionStats = ({question, check}: IProps) => {
  const [open, setOpen] = useState<boolean>(false);

  return (
    <Card classes='stats-card'>
      <strong>{question.question}</strong>
      {question.answerOptions && question.answerOptions.length > 0 &&
        <span className='beginning'>Варіанти відповідей:</span>
      }
      {question.answerOptions && question.answerOptions.length > 0&&
        question.answerOptions.map(answer => (
          <div className='answer-block' onClick={() => check(answer.uuid ?? '')} key={answer.uuid}>
            <span>{answer.optionText} - {answer.selectionCount}{'('+ answer.selectionPercentage+'%)'}</span>
            <div className='progress-bar'>
              <div className='bar' style={{width: answer.selectionPercentage + '%'}}></div>
            </div>
          </div>
        ))
      }
      {question.textAnswers && question.textAnswers.length > 0 &&
      <div className={`own-answers ${open ? 'open': ''}`}>
        <div className='answer-header beginning'>
        <span >Власні відповіді</span>
        <div className='arrow' onClick={()=> setOpen(!open)}></div>
        </div>
        <div className='answer-content'>
        {question.textAnswers.map(answer => (
          <div className='answer-block'>
            <span>{answer.text} - {answer.count} раз{'('}iв{')'}</span>
          </div>))
        }
        </div>
        </div>
      }
    </Card>
  );
}

export default QuestionStats;