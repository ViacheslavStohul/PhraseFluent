import React from 'react';
import './authorization.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../../store';
import { AuthActions } from '../../../store/slice/auth';

const Authorization = () => {
  const dispatch = useDispatch<AppDispatch>();

  const submit = () => {
    dispatch(AuthActions.authFetch({username: '', password: ''}));
  }
  return (
    <form className='authorization'>    
      <h2>Login</h2>
      <div className='fields'>
      <InputFieldComponent labelText='Email' value='xxx@xxx.com' type='email'/>
      <InputFieldComponent labelText='Password' value='********'/>
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