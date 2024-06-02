import React, { useMemo, useState } from 'react';
import Card from '../../../layouts/card/card';
import { useTranslation } from 'react-i18next';
import { ICard,Option } from '../../../../interfaces/test';
import * as langService from '../../../../service/word.service';
import { callErrorToast } from '../../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import { InputFieldComponent } from '../../../fields/input-field/input-field';
import Select from 'react-select';
import { IOption } from '../../../../interfaces/option';
import AnswerCard from '../answer-card/answer-card';

interface IProps {
  emit: (card: ICard) => void;
  testId: string;
}


const CreateQuestionCard = ({emit, testId}: IProps) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [card, setCard] = useState<Partial<ICard>>({});

  const types: IOption[] = useMemo(()=>[
    {value: 'Text', label: t('text').toString()},
    {value: 'TestOneAnswer', label: t('test-one').toString()},
    {value: 'TestManyAnswers', label: t('test-many').toString()},
  ],[t]);

  const createCard = () => {
    langService.createCard({...card, testUuid: testId} as ICard)
    .then(()=>{
      emit({...card, testUuid: testId} as ICard);
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
  };

  const setTextAnswer = (value: string) => {
    setCard((prevCard) => ({
     ...prevCard,
      answerOptions: [
        {
          optionText: value,
          isCorrect: true
        }
      ]
    }));
  }

  const isDisabled = () => {
    return !card.question || 
    card.question.length < 2 || 
    !card.questionType || 
    !card.answerOptions || 
    card.answerOptions.length === 0 || 
    !card.answerOptions.some(option => option.isCorrect) || 
    card.answerOptions.some(option => option.optionText.length < 1);
  }

  const changeOption = (option: Option, index?: number) => {
    setCard((prevCard) => ({
     ...prevCard,
      answerOptions: 
      index !== undefined ? 
        prevCard.answerOptions ? prevCard.answerOptions.map((prevOption, i) => i === index? option : {...prevOption, isCorrect: prevCard.questionType === 'TestOneAnswer' && option.isCorrect ? false : prevOption.isCorrect }) : []
      : [
          ...(prevCard.answerOptions? prevCard.answerOptions.map(prevOption => ({...prevOption, isCorrect: prevCard.questionType === 'TestOneAnswer' && option.isCorrect ? false : prevOption.isCorrect})): []),
          option
        ]
    }));
  
  }

  return (
    <Card classes='new-card'>
      <h2>{t('new-question')}</h2>
      <InputFieldComponent 
        labelText={t("question-text")}
        name='question-text'
        isRequired={true}
        value={card.question??''}
        changed={(value) => handleChange('question', value)}/>
      <div className='input-field'>
        <label
          className="label"
        >
          {t("question-type")}
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
            <AnswerCard emit={changeOption}/>
          </div>
        : card.questionType === 'Text' ?
            <InputFieldComponent 
              labelText={t("answer-text")}
              name='answer'
              value={card?.answerOptions?.[0]?.optionText??''}
              changed={setTextAnswer}/>
        :
        <></>
      }
      <button onClick={createCard} disabled={isDisabled()}>
        {t('create-question')}
      </button>
    </Card>
  );
}

export default CreateQuestionCard;