import React, { useState } from 'react';
import { Test } from '../../../interfaces/test';
import './test-card.scss';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

const TestCard = ({test}:{test:Test}): React.JSX.Element => {
  const [imageError, setImageError] = useState(false);
  const { t } = useTranslation();

  const handleError = (): void => {
   setImageError(true);
  };

  return (
    <div className='test-card'>
      <img 
        alt={test.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': test.imageUrl?? ''}
        onError={handleError}/>
      <div className='test-text'>
        <h4>{test.title}</h4>
        <span>{t('by')} <Link to={`/profile?id=${test.createdBy.uuid}`}>{test.createdBy.username}</Link></span>
        <div className='bottom-test'>
          <div className='chip'>
            {test.language.nativeName}
          </div>
          <div>
            {test.cardsAmount} {t('cards')}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TestCard;
