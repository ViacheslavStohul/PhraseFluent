import React from 'react';
import { Option } from '../../../../interfaces/test';
import './answer-card.scss';
import { InputFieldComponent } from '../../../fields/input-field/input-field';

interface IProps {
  option: Option;
  emit?: (value: Option) => void
}


const AnswerCard = ({option, emit}: IProps) => {

  const setText = (value: string) => {
    if (!emit) return;
      emit({
        ...option,
        optionText: value
      });
  }

  return (
    <div className='answer-card'>
      { emit ?
      <>
      <InputFieldComponent 
       labelText='Текст варіанту'
       name='optiontext'
       isRequired={true}
       value={option.optionText}
       changed={setText}/>
   </>
        :
        <>
          <label>Текст варіанту</label>
          <span>{option?.optionText}</span>
        </>
      }
    </div>
  );
}

export default AnswerCard;