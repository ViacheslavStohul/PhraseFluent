import React from 'react';
import './user.scss';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../../store/slice/auth';
import { Link } from 'react-router-dom';

const User = () => {
  const user = useSelector(AuthSelectors.selectUser);
  return (
    <Link to='/profile'>
    <div className='user'>
      <img alt='profile-pic' src={user?.imageUrl && user?.imageUrl !== null ? user?.imageUrl : 'https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg'}/>
      {user?.username}
    </div>
    </Link>
  );
}

export default User;