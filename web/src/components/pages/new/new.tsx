import React, { useState } from 'react';
import './new.scss';
import Card from '../../layouts/card/card';
import { Test, createTestRequest } from '../../../interfaces/test';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useTranslation } from 'react-i18next';
import PlusSVG from '../../svg/plus';
import { LangFieldComponent } from '../../fields/lang-field/lang-field';
import { Protection } from '../../protection/protection';
import * as langService from '../../../service/word.service';
import { callErrorToast } from '../../../store/slice/toast';
import { useDispatch } from 'react-redux';
import Edit from '../edit/edit';

const NewTest = () => {
  const [newTest, setNewTest] = useState<Partial<createTestRequest>>({});
  const [test, setTest] = useState<Test>();
  const { t } = useTranslation();
  const [imageError, setImageError] = useState(false);
  const [isSubmited, setIsSubmited] = useState<boolean>();
  const dispatch = useDispatch();

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
        setTest(test);
      })
      .catch((error) => {
        setIsSubmited(false);
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.message}));
      });
  };


  return (
    <Protection>
      {test ? 
      <Edit test={test}/>
      :
      <Card classes='new-test'>
      <div className='new-test-fields'>
        <div className='new-test-header'>
        <h2>Create Test</h2>
        <div>
          <label
            className="label"
          >
            {t("language")+'*'}
          </label>
          <LangFieldComponent
            disabled={isSubmited}
            selectLanguage={(option) => handleChange('languageUuid', option?.value)}/>
        </div>
        </div>
        <InputFieldComponent 
          labelText={t("title")}
          name='title'
          isRequired={true}
          readonly={isSubmited}
          value={newTest?.title}
          changed={(value) => handleChange('title', value)}/>
        <InputFieldComponent 
          labelText={t("description")}
          value={newTest?.description}
          readonly={isSubmited}
          name="description"
          changed={(value) => handleChange('description', value)}/>
        <InputFieldComponent 
          labelText={t("test-picture")}
          value={newTest?.imageUrl}
          readonly={isSubmited}
          name="test-picture"
          changed={(value) => handleChange('imageUrl', value)}/>
        <button
          type='button'
          disabled={!newTest.title || !newTest.languageUuid || isSubmited}
          onClick = {createNew}>
            <PlusSVG/>
          {t("create-test")}
        </button>
      </div>
      <img 
        alt={newTest?.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': newTest?.imageUrl?? ''}
        onError={handleError}/>
    </Card>
    }
    
    </Protection>
  );
}

export default NewTest;