import React, { useState } from 'react';
import { Test } from '../../../interfaces/test';
import './test-card.scss';

const TestCard = ({test}:{test:Test}): React.JSX.Element => {
  const [imageError, setImageError] = useState(false);

  const handleError = (): void => {
   setImageError(true);
  };

  return (
    <div className='test-card'>
      <img 
        alt={test.title}
        src={imageError ? test.imageUrl : 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg'}
        onError={handleError}/>
      <div className='test-text'>
        <h4>{test.title}</h4>
        <span>by {test.createdBy.username}</span>
        <div className='bottom-test'>
          <div className='chip'>
            {test.language.title}
          </div>
          <div>
            {test.cardsAmount} Cards
          </div>
        </div>
      </div>
    </div>
  );
};

export default TestCard;
