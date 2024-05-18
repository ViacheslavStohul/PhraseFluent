import React, { ReactNode } from 'react';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../store/slice/auth';
import { useNavigate } from 'react-router-dom';

export const Protection = ({children}:{children: ReactNode})=> {
  const user = useSelector(AuthSelectors.selectUser);
  const navigate = useNavigate();

  if (!user) {
    navigate('/');
  }

  return (
    <>
    {children}
    </>
  );
};