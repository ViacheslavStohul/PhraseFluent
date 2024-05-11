import React, { ReactNode } from 'react';
import './card.scss';

const Card = (
  {children, classes}:
  {children:ReactNode, classes?: string}) => {
  return (
    <div className={`card ${classes}`}>
      {children}
    </div>
  );
}

export default Card;