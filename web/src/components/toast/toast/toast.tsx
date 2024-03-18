import React, { useEffect } from 'react';
import './toast.scss';
import { ToastType } from '../../../enum/toast';
import { useDispatch } from 'react-redux';
import { IToast } from '../../interfaces/toast';
import CloseSVG from '../../svg/close';
import { removeToast } from '../../../store/slice/toast';

const Toast = ({toast}:{toast:IToast}) => {
  const dispatch = useDispatch();
  const duration = 6000;

  useEffect(() => {
    const timer = setTimeout(() => {
      dispatch(removeToast(toast.id));
    }, duration);

    return () => {
      clearTimeout(timer);
    };
  }, [dispatch, toast, duration]);

  const remove = () => {
    dispatch(removeToast(toast.id));
  }

  return (
    <div className={`toast ${ToastType[toast.type]}`} style={{animationDuration: duration / 1000 + 's'}}>
      <div className='toast-header'>
        {toast.name}
        <div onClick={remove} className='pointer'><CloseSVG/></div>
      </div>
      {toast.text}
    </div>
  );
}

export default Toast;