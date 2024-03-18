import React from 'react';
import './user.scss';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../../store/slice/auth';

const User = () => {
  const user = useSelector(AuthSelectors.selectUsername);
  return (
    <div className='user'>
      <img alt='profile-pic' src='https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg'/>
      {user}
    </div>
  );
}

export default User;