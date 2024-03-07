import React, { useContext, useEffect } from 'react';
import './toast.scss';
import { IToast, ToastContext } from '../toasts';

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

  return (
    <div>
      {toast.text}
    </div>
  );
}

export default Toast;