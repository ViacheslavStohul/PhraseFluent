import React, { useMemo, useState } from 'react';
import Card from '../../../layouts/card/card';
import { useTranslation } from 'react-i18next';
import { ICard } from '../../../../interfaces/test';
import * as langService from '../../../../service/word.service';
import { callErrorToast } from '../../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import { InputFieldComponent } from '../../../fields/input-field/input-field';
import Select from 'react-select';
import { IOption } from '../../../../interfaces/option';

interface IProps {
  emit: (card: ICard) => void;
}


const CreateTestCard = ({emit}: IProps) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [card, setCard] = useState<Partial<ICard>>({});

  const types: IOption[] = useMemo(()=>[
    {value: 'Text', label: t('text').toString()},
    {value: 'TestOneAnswer', label: t('test-one').toString()},
    {value: 'TestManyAnswers', label: t('test-many').toString()},
  ],[t]);

  const createCard = () => {
    langService.createCard(card as ICard)
    .then((response)=>{
      emit(response);
      setCard({});
    })
    .catch((error) => {
      dispatch(callErrorToast({name: error.code, text: error.message}));
    });
  }

  const handleChange = (
    key: string,
    value: string | number
  ): void => {
    setCard((prevCard) => ({
      ...prevCard,
      [key]: value
    }));
  };

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
      <button onClick={createCard}>
        {t('create-question')}
      </button>
    </Card>
  );
}

export default CreateTestCard;