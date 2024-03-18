import React, { useEffect, useState } from 'react';
import './authorization.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';
import { IAuth } from '../../interfaces/auth';
import { useNavigate } from 'react-router-dom';

const initialState: IAuth = {
  username: '',
  password: ''
}

const Authorization = () => {
  const dispatch = useDispatch();
  const user = useSelector(AuthSelectors.selectUsername);
  const navigate = useNavigate();

  const [form, setForm] = useState<IAuth>(initialState);

  useEffect(()=>{
    if (user) {
      navigate('/');
    }
  },[user,navigate]);

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