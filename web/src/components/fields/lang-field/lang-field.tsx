import React, { useEffect, useState } from 'react';
import Select from 'react-select';
import { IOption } from '../../../interfaces/option';
import * as langService from '../../../service/word.service';
import { useDispatch } from 'react-redux';
import { callErrorToast } from '../../../store/slice/toast';
import { useTranslation } from 'react-i18next';

export interface ILangFieldProps {
  selectLanguage: (value: IOption) => void;
  disabled?: boolean
}

export const LangFieldComponent = (
  props: ILangFieldProps
): React.JSX.Element => {
  const { selectLanguage, disabled } = props;
  const [langs, setLangs] = useState<IOption[]>([]);
  const { t } = useTranslation();
  const dispatch = useDispatch();



  useEffect(()=>{
    langService.getLangs()
    .then(languages=>{
      setLangs(languages.map(lang =>{
        return {
          value: lang.uuid,
          label: lang.title+' (' + lang.nativeName+')'
        }
      }));
    })
    .catch((error) => dispatch(callErrorToast({name: error.code, text: error.message})));
  },[dispatch])

  return (
    <Select
    classNamePrefix='select'
    isDisabled={disabled}
    className='select'
    aria-label='language'
    placeholder={t("select-language")}
    options={langs}
    onChange={(value) => selectLanguage(value as IOption)}/>
  );
};