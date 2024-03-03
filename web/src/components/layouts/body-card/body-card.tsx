import React from 'react';
import { Outlet } from 'react-router-dom';
import Card from '../card/card';

const BodyCard = () => {
  return (
    <Card>
      <Outlet/>
    </Card>
  );
}

export default BodyCard;