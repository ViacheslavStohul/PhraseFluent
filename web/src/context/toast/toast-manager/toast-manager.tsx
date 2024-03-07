import React, { useContext } from 'react';
import './toast-manager.scss';
import { IToast, ToastContext } from '../toasts';
import Toast from '../toast/toast';

const ToastManager = () => {
  const {toasts} = useContext(ToastContext);
  return (
    <div className='toast-manager'>
      <div className='toast-body'>
        {toasts && toasts.map((toast: IToast) => 
          <Toast key={toast.id} toast={toast}/>
        )}
      </div>
    </div>
  );
}

export default ToastManager;