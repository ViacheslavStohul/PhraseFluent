import React from 'react';
import { Option } from '../../../../interfaces/test';
import './option.scss';

interface IProps {
  option: Option;
  emit: () => void
}


const OptionCard = ({option, emit}: IProps) => {
  
  return (
    <div className={`answer-card ${option?.isCorrect ? 'correct' : ''}`} onClick={()=>emit()}>
      <span>{option?.optionText}</span>
    </div>
  );
}

export default OptionCard;