import { ToastType } from "../enum/toast";

export interface IToast {
  id: number;
  name: string;
  text: string;
  type: ToastType;
}
