import React from 'react';
import { ICard} from '../../../../interfaces/test';
import './question-stats.scss';
import Card from '../../../layouts/card/card';

interface IProps {
  question: ICard;
}


const QuestionStats = ({question}: IProps) => {

  return (
    <Card classes='stats-card'>
      <strong>{question.question}</strong>
      {question.answerOptions && question.answerOptions.length > 0 &&
        <span>Варіанти відповідей:</span>
      }
      {question.answerOptions && question.answerOptions.length > 0&&
        question.answerOptions.map(answer => (
          <div className='answer-block'>
            <span>{answer.optionText} - {answer.selectionCount}{'('+ answer.selectionPercentage+'%)'}</span>
            <div className='progress-bar'>
              <div className='bar' style={{width: answer.selectionPercentage + '%'}}></div>
            </div>
          </div>
        ))
      }
      {question.textAnswers && question.textAnswers.length > 0 &&
      <span>Власні відповідей:</span>
      }
      {question.textAnswers && question.textAnswers.length > 0 &&
        question.textAnswers.map(answer => (
          <div className='answer-block'>
            <span>{answer.optionText} - {answer.selectionCount}{'('+ answer.selectionPercentage+'%)'}</span>
            <div className='progress-bar'>
              <div className='bar' style={{width: answer.selectionPercentage + '%'}}></div>
            </div>
          </div>
        ))
      }
    </Card>
  );
}

export default QuestionStats;