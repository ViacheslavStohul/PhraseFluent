import React from 'react';
import { Option } from '../../../../interfaces/test';
import './answer-card.scss';
import { InputFieldComponent } from '../../../fields/input-field/input-field';

interface IProps {
  option: Option;
  onChange?: (value: Option) => void;
  onDelete?: () => void;
}


const AnswerCard = ({option, onDelete, onChange}: IProps) => {

  const setText = (value: string) => {
    if (!onChange) return;
    onChange({
        ...option,
        optionText: value
      });
  }

  return (
    <div className='answer-card'>
      { onChange && onDelete ?
      <>
      <InputFieldComponent 
       labelText='Текст варіанту'
       name='optiontext'
       isRequired={true}
       value={option.optionText}
       changed={setText}/>
       <button onClick={() => onDelete()}>Видалити</button>
   </>
        :
        <div className='finished-answer'>
          <label>Текст варіанту</label>
          <span>{option?.optionText}</span>
        </div>
      }
    </div>
  );
}

export default AnswerCard;