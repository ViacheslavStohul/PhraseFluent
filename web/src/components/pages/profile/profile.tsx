import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';
import TestList from '../../test-list/test-list';
import './profile.scss';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { IUser } from '../../../interfaces/auth';
import * as AuthService from '../../../service/auth.service';
import { callErrorToast } from '../../../store/slice/toast';
import Card from '../../layouts/card/card';
import { InputFieldComponent } from '../../fields/input-field/input-field';
import { Protection } from '../../protection/protection';

const Profile = () => {

  const userStore = useSelector(AuthSelectors.selectUser);
  const [user, setUser] = useState<IUser>();
  const { t } = useTranslation();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [imageError, setImageError] = useState(false);

  const handleError = (): void => {
   setImageError(true);
  };


  useEffect(()=> {
    if (!userStore){
      navigate('/');
      return;
    }

    if (searchParams.get('id') && searchParams.get('id') !== null && searchParams.get('id') !== userStore.uuid){
      AuthService.Get(searchParams.get('id')?.toString())
      .then(user=>{
        setUser(user);
      })
      .catch((error) => dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.message})));
    } else {
      setUser(userStore);
    }

  },[userStore, navigate, searchParams,dispatch]);

  const updateImage = (link: string) => {
    setImageError(false);
    dispatch(AuthActions.updateUserImageFetch(link));
  }

  
  return (
    <Protection>
    <div className='profile'>
    {
      user ?
      <>
      <TestList title={user.uuid === userStore?.uuid ? t('my-tests'): t('user-tests')} user={user}/>
      <Card classes='profile-content'>
        <img 
          alt={user.username}
          src={imageError ? 'https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg': user.imageUrl?? ''}
          onError={handleError}/>
        <h2>{user.username}</h2>
        <div>{t('joined')} {new Date(user.registrationDate).toLocaleDateString()}</div>
        { userStore?.uuid === user.uuid &&
        <InputFieldComponent 
          labelText={t("picture")}
          name='picture'
          value={user.imageUrl?? ''}
          changed={updateImage}/>
        }
      </Card>
      </>
      :
      <></>
    }
    </div>
    </Protection>
  );
}

export default Profile;