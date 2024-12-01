import React, { useState } from 'react';
import { Option } from '../../../../interfaces/test';
import './answer-card.scss';
import { useTranslation } from 'react-i18next';
import { InputFieldComponent } from '../../../fields/input-field/input-field';

interface IProps {
  option?: Option;
  emit?: (value: Option) => void
}


const AnswerCard = ({option, emit}: IProps) => {
  const {t} = useTranslation();
  const [newOption, setNewOption] = useState<Option>({optionText: '', isCorrect: false});

  const setText = (value: string) => {
    if (!emit) return;
    if (option){
      emit({
        ...option,
        optionText: value
      });
    } else {
      setNewOption({
        ...newOption, optionText: value
      });
    }
  }

  const setCorrect = () => {
    if (!emit) return;
    if (option){
      emit({
        ...option,
        isCorrect: !option.isCorrect
      });
    } else {
      setNewOption({...newOption, isCorrect: !newOption.isCorrect});
    }
  }

  const addAnswer = () => {
    if (emit){
      emit(newOption);
      setNewOption({optionText: '', isCorrect: false});
    }
  }

  return (
    <div className={`answer-card ${option?.isCorrect ? 'correct' : ''}`}>
      { emit ?
      <>
      <InputFieldComponent 
       labelText={t("option-text")}
       name='optiontext'
       isRequired={true}
       value={option ? option.optionText : newOption.optionText}
       changed={setText}/>
         <button 
           type='button' 
           className={`checkbox ${option?.isCorrect || newOption.isCorrect ? 'active': ''}`}
           onClick={setCorrect}><div className='check'></div><span>{t('is-correct')}</span></button>
       {
         !option &&
         <button type='button'
         onClick={addAnswer}
         disabled={newOption.optionText.length < 1}
         >{t('add-answer')}</button>
       }
   </>
        :
        <>
          <label>{t('option-text')}</label>
          <span>{option?.optionText}</span>
        </>
      }
    </div>
  );
}

export default AnswerCard;