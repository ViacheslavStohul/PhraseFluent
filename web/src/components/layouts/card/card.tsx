import React, { ReactNode } from 'react';
import './card.scss';

const Card = (
  {children}:
  {children:ReactNode}) => {
  return (
    <div className='card'>
      {children}
    </div>
  );
}

export default Card;