import React, { useState } from 'react';
import { Test } from '../../../interfaces/test';
import './test-card.scss';
import { Link, useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';

const TestCard = ({test}:{test:Test}): React.JSX.Element => {
  const [imageError, setImageError] = useState(false);
  const user = useSelector(AuthSelectors.selectUser);
  const navigate = useNavigate();

  const handleError = (): void => {
   setImageError(true);
  };

  const toTest = (): void => {
    navigate(`/test?id=${test.uuid}`);
  };

  return (
    <div className='test-card'>
      <img
        onClick={toTest} 
        alt={test.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': test.imageUrl?? ''}
        onError={handleError}/>
      <div className='test-text'>
        <div className='test-text-block' onClick={toTest}>
        <h4>{test.title}</h4>
        <p>{test.description}</p>
        </div>
        <div className='bottom'>
        { user &&
        <>
        <Link to={`/statistics?id=${test.uuid}`}>Переглянути статистику</Link>
        <Link to={`/edit?id=${test.uuid}`}>Редагувати</Link>
        </>
        }
        <Link className='link' to={`/test?id=${test.uuid}`}>Пройти опитування</Link>
        </div>
      </div>
    </div>
  );
};

export default TestCard;
