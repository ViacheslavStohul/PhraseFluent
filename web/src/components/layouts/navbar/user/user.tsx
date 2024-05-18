import React, { useEffect, useState } from 'react';
import './user.scss';
import { useSelector } from 'react-redux';
import { AuthSelectors } from '../../../../store/slice/auth';
import { Link } from 'react-router-dom';

const User = () => {
  const user = useSelector(AuthSelectors.selectUser);
  const [imageError, setImageError] = useState(false);

  const handleError = (): void => {
   setImageError(true);
  };

  useEffect(()=>{
    if(user?.imageUrl){
      setImageError(false);
    }else{
      setImageError(true);
    }
  },[user?.imageUrl]);
  
  return (
    <Link to='/profile'>
    <div className='user'>
      <img alt='profile-pic'           
        src={imageError ? 'https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg': user?.imageUrl?? ''}
        onError={handleError}/>
        {user?.username}
    </div>
    </Link>
  );
}

export default User;