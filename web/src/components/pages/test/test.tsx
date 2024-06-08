import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AuthSelectors } from '../../../store/slice/auth';
import { useNavigate, useSearchParams } from 'react-router-dom';
import * as testService from '../../../service/word.service';
import { callErrorToast } from '../../../store/slice/toast';

const User = () => {
  const user = useSelector(AuthSelectors.selectUser);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    if (!user){
      navigate('/');
    }

    let id = searchParams.get('id');
    if (id) {
      testService.beginTest(id).then((res)=>{
        console.log(res);
      }).catch((error) => {
        dispatch(callErrorToast({name: error.code, text: error.response?.data?.Message ?? error.response?.data?.Message ?? error.message}));
      });
    } else {
      navigate('/');
      return;
    }
  },[user, navigate, searchParams, dispatch]);
  
  return (
    <div>
      
    </div>
  );
}

export default User;