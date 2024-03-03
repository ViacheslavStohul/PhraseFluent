import React from 'react';
import './authorization.scss';
import { InputFieldComponent } from '../../fields/input-field/input-field';

const Authorization = () => {
  return (
    <form className='authorization'>    
      <h2>Login</h2>
      <div className='fields'>
      <InputFieldComponent labelText='Email' value='xxx@xxx.com' type='email'/>
      <InputFieldComponent labelText='Password' value='********'/>
      </div>
      <button
      type='submit'>
        Login
      </button>
    </form>
  );
}

export default Authorization;