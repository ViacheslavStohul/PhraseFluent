import React, { useState } from 'react';
import './registration.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../../store';
import { AuthActions } from '../../../store/slice/auth';
import { IRegister } from '../../interfaces/auth';

const initialState: IRegister = {
  username: '',
  password: '',
  repeatedPassword: ''
}

const Registration = () => {
  const dispatch = useDispatch<AppDispatch>();

  const [form, setForm] = useState<IRegister>(initialState);


  const submit = () => {
    dispatch(AuthActions.registerFetch(form));
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
    <form className='registration'>    
      <h2>Registration</h2>
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
      <InputFieldComponent 
        labelText='Repeat password' 
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
        Login
      </button>
    </form>
  );
}

export default Registration;