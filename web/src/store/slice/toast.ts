import { createSlice } from '@reduxjs/toolkit';
import { IToast } from '../../components/interfaces/toast';
import { ToastType } from '../../enum/toast';


const initialState: {toasts: IToast[]} = {
  toasts: []};

export const toastSlice = createSlice({
  initialState,
  name: 'toasts',
  reducers: (create) => ({
    callToast: create.reducer<Pick<IToast, 'name'| 'text'| 'type'>>((state, action) => {
      state.toasts.unshift({
        id: Math.floor(Math.random()*1000000), 
        name: action.payload.name, 
        text: action.payload.text,
        type: action.payload.type
      });
    }),
    callErrorToast: create.reducer<Pick<IToast, 'name'| 'text'>>((state, action) => {
      const toast: IToast = {
        id: Math.floor(Math.random()*1000000), 
        name: action.payload.name, 
        text: action.payload.text,
        type: ToastType.Error
      };
      state.toasts.unshift(toast);
    }),
    removeToast: create.reducer<number>((state, action) => {
      state.toasts = state.toasts.filter(
        (toast) => toast.id !== action.payload
      );
    })
  }),
  selectors: {
    selectToasts: (state) => state.toasts,
}});


export const { callToast, removeToast, callErrorToast } = toastSlice.actions;
export const { selectToasts } = toastSlice.selectors;