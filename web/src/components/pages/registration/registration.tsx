import React, { useEffect, useState } from 'react';
import './registration.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';
import { IRegister } from '../../interfaces/auth';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const initialState: IRegister = {
  username: '',
  password: '',
  repeatedPassword: ''
}

const Registration = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const user = useSelector(AuthSelectors.selectUsername);
  const navigate = useNavigate();

  const [form, setForm] = useState<IRegister>(initialState);

  const submit = () => {
    dispatch(AuthActions.registerFetch(form));
  }

  useEffect(()=>{
    if (user) {
      navigate('/');
    }
  },[user,navigate]);

  const handleChange = (
    key: string,
    value: string | boolean
  ): void => {
      setForm((prevForm) => ({
        ...prevForm,
        [key]: value
      }));
  };


  return (
    <form className='registration'>    
      <h2>{t("registration")}</h2>
      <div className='fields'>
      <InputFieldComponent 
        labelText={t("username")}
        name='username'
        value={form.username}
        changed={(value) => handleChange('username', value)}/>
      <InputFieldComponent 
        labelText={t("password")}
        value={form.password}
        type="password" 
        name="password"
        changed={(value) => handleChange('password', value)}/>
      <InputFieldComponent 
        labelText={t("repeatPassword")}
        value={form.repeatedPassword}
        type="password" 
        name="repeatedPassword"
        changed={(value) => handleChange('repeatedPassword', value)}/>
        {form.password !== form.repeatedPassword && 
        <div className='error-message'>Passwords must be the same</div>}
      </div>
      <button
      type='button'
      onClick = {submit}>
        {t("register")}
      </button>
    </form>
  );
}

export default Registration;