import React, { useState } from 'react';
import './edit.scss';
import '../new/new.scss';
import { ICard, Test } from '../../../interfaces/test';
import Card from '../../layouts/card/card';
import CreateTestCard from './create-test-card/create-test-card';

const Edit = ({test}:{test: Test}) => {
  // const { t } = useTranslation();
  const [imageError, setImageError] = useState(false);
  const [cards, setCards]=useState<ICard[]>([]);

  const handleError = (): void => {
   setImageError(true);
  };

  const addQuestion = (question: ICard): void => {
    setCards([...cards, question]);
  }

  
  return (
    <div className='new-edit'>
      <Card classes='new-test'>
        <div className='new-test-text'>
          <h2>{test.title}</h2>
          <label>{test.language.nativeName+'('+test.language.title+')'}</label>
          <p>{test.description}</p>
        </div>
        <img 
        alt={test?.title}
        src={imageError ? 'https://img.freepik.com/premium-vector/default-image-icon-vector-missing-picture-page-website-design-mobile-app-no-photo-available_87543-11093.jpg': test?.imageUrl?? ''}
        onError={handleError}/>
      </Card>
      {
        cards.map((card) => (
          <Card>
            {card.question}
          </Card>
        ))
      }
      <CreateTestCard emit={addQuestion}/>
    </div>
  );
}

export default Edit;