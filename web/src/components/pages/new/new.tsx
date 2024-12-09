import React, { useState } from 'react';
import './new.scss';
import Card from '../../layouts/card/card';
import { createTestRequest } from '../../../interfaces/test';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import PlusSVG from '../../svg/plus';
import { Protection } from '../../protection/protection';
import * as langService from '../../../service/word.service';
import { callErrorToast } from '../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const NewTest = () => {
  const [newTest, setNewTest] = useState<Partial<createTestRequest>>({});
  const [imageError, setImageError] = useState(false);
  const [isSubmited, setIsSubmited] = useState<boolean>();
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleError = (): void => {
   setImageError(true);
  };

  const handleChange = (
    key: string,
    value: string | number
  ): void => {
    if (key === 'imageUrl'){
      setImageError(false);
    }
    setNewTest((prevTest) => ({
      ...prevTest,
      [key]: value
    }));
  };

  const createNew = (): void => {
    setIsSubmited(true);
    langService.createTest(newTest as createTestRequest)
     .then((test) => {
        navigate(`/edit?id=${test.uuid}`);
      })
      .catch((error) => {
        setIsSubmited(false);
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.message}));
      });
  };


  return (
    <Protection>
      <Card classes='new-test'>
      <div className='new-test-fields'>
        <div className='new-test-header'>
        <h2>Створення опитування</h2>
        </div>
        <InputFieldComponent 
          labelText='Заголовок'
          name='title'
          isRequired={true}
          readonly={isSubmited}
          value={newTest?.title}
          changed={(value) => handleChange('title', value)}/>
        <InputFieldComponent 
          labelText='Опис'
          value={newTest?.description}
          readonly={isSubmited}
          name="description"
          changed={(value) => handleChange('description', value)}/>
        <InputFieldComponent 
          labelText='Посилання на зображення'
          value={newTest?.imageUrl}
          readonly={isSubmited}
          name="test-picture"
          changed={(value) => handleChange('imageUrl', value)}/>
          <div>
        <button
          type='button'
          disabled={!newTest.title || isSubmited}
          onClick = {createNew}>
            <PlusSVG/>
          Створити опитування
        </button>
        </div>
      </div>
      <img 
        alt={newTest?.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': newTest?.imageUrl?? ''}
        onError={handleError}/>
    </Card>
    </Protection>
  );
}

export default NewTest;