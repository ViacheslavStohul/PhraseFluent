import React from 'react';
import { Outlet } from 'react-router-dom';
import Card from '../../card/card';
import './form-card.scss';

const FormCard = () => {
  return (
    <div className='form-card'>
    <Card>
      <Outlet/>
    </Card>
    </div>
  );
}

export default FormCard;