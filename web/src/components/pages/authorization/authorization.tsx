import React, { useContext } from 'react';
import './authorization.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import * as AuthService from '../../../service/auth.service';
import { ToastContext } from '../../../context/toast/toasts';
import { ToastType } from '../../../enum/toast';

const Authorization = () => {
  const {addToast} = useContext(ToastContext);

  const submit = () => {
    AuthService.Auth().then((value)=>{
      console.log(value);
    })
    .catch((error) => {
      addToast(error.code, error.message, ToastType.Error);
    });
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