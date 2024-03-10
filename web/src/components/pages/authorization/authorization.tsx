import React, { useState } from 'react';
import './authorization.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../../store';
import { AuthActions } from '../../../store/slice/auth';
import { IAuth } from '../../interfaces/auth';

const initialState: IAuth = {
  username: '',
  password: ''
}

const Authorization = () => {
  const dispatch = useDispatch<AppDispatch>();

  const [form, setForm] = useState<IAuth>(initialState);


  const submit = () => {
    dispatch(AuthActions.authFetch(form));
  }

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
    <form className='authorization'>    
      <h2>Login</h2>
      <div className='fields'>
      <InputFieldComponent 
        labelText='Username'
        name='username'
        value={form.username}
        changed={(value) => handleChange('username', value)}/>
      <InputFieldComponent 
        labelText='Password' 
        value={form.password}
        type="password" 
        name="password"
        changed={(value) => handleChange('password', value)}/>
      </div>
      <button
      type='button'
      onClick = {submit}>
        Login
      </button>
    </form>
  );
}

export default Authorization;