import React from 'react';
import './toast-manager.scss';
import Toast from '../toast/toast';
import { selectToasts } from '../../../store/slice/toast';
import { useSelector } from 'react-redux';
import { IToast } from '../../interfaces/toast';

const ToastManager = () => {
  const toasts = useSelector(selectToasts);

  return (
    <div className='toast-manager'>
        {toasts && toasts.map((toast: IToast) => 
          <Toast key={toast.id} toast={toast}/>
        )}
    </div>
  );
}

export default ToastManager;