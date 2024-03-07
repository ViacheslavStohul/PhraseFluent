import React, { useState, createContext, useCallback } from "react";
import { ToastType } from "../enum/toast";


export interface Toast {
  id: number;
  name: string;
  text: string;
  type: ToastType;
}

const ToastContext = createContext<any>(null);

export const ToastProvider = (
  { children }: {
  children: React.ReactNode;
}) => {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const removeToast = useCallback((id: number) => {
    setToasts(prev => prev.filter(toast => toast.id === id));
  },[]);

  const addToast = useCallback((name: string, text: string, type: ToastType) => {
    setToasts(prev => {
      return [{id: (Math.random()*1000), name, text, type},...prev];
    });
  },[]);

  return (
    <ToastContext.Provider value={{ toasts, addToast, removeToast }}>
      {children}
    </ToastContext.Provider>
  );
};