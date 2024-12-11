import React, { useMemo, useState } from 'react';
import Card from '../../../layouts/card/card';
import { ICard,Option } from '../../../../interfaces/test';
import * as testService from '../../../../service/test.service';
import { callErrorToast } from '../../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import { InputFieldComponent } from '../../../fields/input-field/input-field';
import Select from 'react-select';
import AnswerCard from '../answer-card/answer-card';
import Checkbox from '../../../fields/checkbox/checkbox';
import { types } from '../../../../const/types';

interface IProps {
  emit: (card: ICard) => void;
  testId: string;
}


const CreateQuestionCard = ({emit, testId}: IProps) => {
  const dispatch = useDispatch();
  const [card, setCard] = useState<Partial<ICard>>({});
  const [custom, setCustom] = useState<boolean>(false);

  const createCard = () => {
    const newCard = card;
    if (custom)
      newCard.answerOptions = [...(newCard.answerOptions ?? []), {optionText: 'Інше:', isAllowedText: true} ];
    testService.createCard({...newCard, testUuid: testId} as ICard).then(()=>{
      emit({...newCard, testUuid: testId} as ICard);
      setCard(prev => ({questionType: prev.questionType}));
    }).catch((error) => dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message})));
  }

  const handleChange = (
    key: string,
    value: string | number
  ): void => {
    setCard((prevCard) => ({
      ...prevCard,
      [key]: value,
      answerOptions: key === 'questionType' && value !== prevCard.questionType ? []: prevCard.answerOptions
    }));
    if (key === 'questionType'){
      setCustom(false);
    }
  };

  const isDisabled = useMemo(() => 
    !card.question || 
    card.question.length < 2 || 
    !card.questionType ||
    ( card.questionType !== 'Text' &&
    (!card.answerOptions || 
    card.answerOptions.length === 0 ||
    card.answerOptions.some(option => option.optionText.length < 1))
  ),[card]);

  const changeOption = (option: Option, index?: number) => {
    setCard((prevCard) => ({
     ...prevCard,
      answerOptions: 
      index !== undefined ? 
        prevCard.answerOptions ? prevCard.answerOptions.map((prevOption, i) => i === index? option : prevOption) : []
      : [
          ...(prevCard.answerOptions ?? []),
          option
        ]
    }));
  
  }

  const deleteOption = (index: number) => {
    setCard((prevCard) => ({
      ...prevCard,
       answerOptions: prevCard.answerOptions ?  prevCard.answerOptions.filter((_, i)=> index !== i): []
     }));
  }


  return (
    <Card classes='new-card'>
      <h2>Нове питання</h2>
      <InputFieldComponent 
        labelText='Текст запитання'
        name='question-text'
        isRequired={true}
        value={card.question??''}
        changed={(value) => handleChange('question', value)}/>
      <div className='input-field'>
        <label
          className="label"
        >
          Тип питання
        </label>
        <Select
          classNamePrefix='select'
          className='select'
          aria-label='type'
          placeholder={''}
          value={types.find(type => type.value === card.questionType)}
          options={types}
          onChange={(value) => handleChange('questionType',value?.value??'')}/>
        </div>
      { (card.questionType === 'TestOneAnswer' || card.questionType === 'TestManyAnswers') &&
          <div className='answer-grid'>
            { card.answerOptions && card.answerOptions.map((option, index)=> (
                <AnswerCard option={option} onChange={(value)=> changeOption(value, index)} onDelete={()=> deleteOption(index)} key={index}/>))}
          </div>
      }
      { (card.questionType === 'TestOneAnswer' || card.questionType === 'TestManyAnswers') &&
          <Checkbox label='Додати власну відповідь' checked={custom} onChange={() => setCustom(!custom)}/>
      }
      <div className='buttons'>
        { (card.questionType === 'TestOneAnswer' || card.questionType === 'TestManyAnswers') &&
        <button onClick={()=> changeOption({optionText:''})}>
          Додати варіант відповіді
        </button>
        }
      <button onClick={createCard} disabled={isDisabled}>
        Створити питання
      </button>
      </div>
    </Card>
  );
}

export default CreateQuestionCard;