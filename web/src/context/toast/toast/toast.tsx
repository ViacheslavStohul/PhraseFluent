import React, { useContext, useEffect } from 'react';
import './toast.scss';
import { IToast, ToastContext } from '../toasts';
import { ToastType } from '../../../enum/toast';
import CloseSVG from '../../../components/svg/close';

const Toast = ({toast}:{toast:IToast}) => {
  const {removeToast} = useContext(ToastContext);
  const duration = 6000;

  useEffect(() => {
    if (removeToast){
    const timer = setTimeout(() => {
      removeToast(toast.id);
    }, duration);

    return () => {
      clearTimeout(timer);
    };
    }
  }, [removeToast, toast, duration]);

  const remove = () => {
    removeToast(toast.id);
  }

  return (
    <div className={`toast ${ToastType[toast.type]}`} style={{animationDuration: duration / 1000 + 's'}}>
      <div className='toast-header'>
        {toast.name}
        <div onClick={remove}><CloseSVG/></div>
      </div>
      {toast.text}
    </div>
  );
}

export default Toast;