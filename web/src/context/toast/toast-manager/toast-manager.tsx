import React, { useContext } from 'react';
import './toast-manager.scss';
import { IToast, ToastContext } from '../toasts';
import Toast from '../toast/toast';

const ToastManager = () => {
  const {toasts} = useContext(ToastContext);
  return (
    <div className='toast-manager'>
        {toasts && toasts.map((toast: IToast) => 
          <Toast key={toast.id} toast={toast}/>
        )}
    </div>
  );
}

export default ToastManager;