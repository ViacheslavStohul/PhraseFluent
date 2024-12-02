import React, { useMemo, useState } from 'react';
import Card from '../../../layouts/card/card';
import { ICard,Option } from '../../../../interfaces/test';
import * as langService from '../../../../service/word.service';
import { callErrorToast } from '../../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import { InputFieldComponent } from '../../../fields/input-field/input-field';
import Select from 'react-select';
import { IOption } from '../../../../interfaces/option';
import AnswerCard from '../answer-card/answer-card';
import Checkbox from '../../../fields/checkbox/checkbox';

interface IProps {
  emit: (card: ICard) => void;
  testId: string;
}


const CreateQuestionCard = ({emit, testId}: IProps) => {
  const dispatch = useDispatch();
  const [card, setCard] = useState<Partial<ICard>>({});
  const [custom, setCustom] = useState<boolean>(false);

  const types: IOption[] = useMemo(()=>[
    {value: 'Text', label: 'Текст'},
    {value: 'TestOneAnswer', label: 'Одна відповідь'},
    {value: 'TestManyAnswers', label: 'Багато відповідей'},
  ],[]);

  const createCard = () => {
    const newCard = card;
    if (custom){
      newCard.answerOptions = [...(newCard.answerOptions ?? []), {optionText: 'Інше:', isAllowedText: true} ];
    }
    langService.createCard({...newCard, testUuid: testId} as ICard)
    .then(()=>{
      emit({...newCard, testUuid: testId} as ICard);
      setCard({});
    })
    .catch((error) => {
      dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
    });
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

  const isDisabled = () => {
    return !card.question || 
    card.question.length < 2 || 
    !card.questionType ||
    ( card.questionType !== 'Text' &&
    (!card.answerOptions || 
    card.answerOptions.length === 0 ||
    card.answerOptions.some(option => option.optionText.length < 1)));
  }

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
          options={types}
          onChange={(value) => handleChange('questionType',value?.value??'')}/>
        </div>
      {
        card.questionType === 'TestOneAnswer' || card.questionType === 'TestManyAnswers'?
          <div className='answer-grid'>
            {
              card.answerOptions && card.answerOptions.map((option, index)=> (
                <AnswerCard option={option} emit={(value)=> changeOption(value, index)} key={index}/>
              ))
            }
          </div>
        :
        <></>
      }
        { card.questionType === 'TestManyAnswers' &&
        <Checkbox label='Додати власну відповідь' checked={custom} onChange={() => setCustom(!custom)}/>
        }
      <div className='buttons'>
        { (card.questionType === 'TestOneAnswer' || card.questionType === 'TestManyAnswers') &&
        <button onClick={()=> changeOption({optionText:''})}>
          Додати варіант відповіді
        </button>
        }
      <button onClick={createCard} disabled={isDisabled()}>
        Створити питання
      </button>
      </div>
    </Card>
  );
}

export default CreateQuestionCard;