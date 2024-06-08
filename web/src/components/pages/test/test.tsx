import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';
import { useNavigate } from 'react-router-dom';

const User = () => {
  const user = useSelector(AuthSelectors.selectUser);
  const navigate = useNavigate();


  useEffect(() => {
    if (!user){
      navigate('/');
    }
  },[user, navigate]);
  
  return (
    <div>
      
    </div>
  );
}

export default User;