import React, { ChangeEvent, useEffect, useRef } from 'react';
import './input-field.scss';

export interface IInputFieldProps {
  labelText: string;
  value: string | number;
  changed?: (value: string) => void;
  readonly?: boolean;
  type?: string;
  isRequired?: boolean;
  placeholder?: string;
  regex?: RegExp;
  focus?: boolean;
}

export const InputFieldComponent = (
  props: IInputFieldProps
): React.JSX.Element => {
  const { labelText, value, changed, readonly, type, isRequired, placeholder, regex, focus } =
    props;
  const inputRef = useRef<HTMLInputElement|null>(null);

  const onChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (changed){
      if (regex?.test(event.target.value) || !regex){
        changed(event.target.value);
      }
    }
  };

  useEffect(()=>{
    if (focus && inputRef.current){
      inputRef.current.focus();
    }
    else if (inputRef.current){
      inputRef.current.blur();
    }
  },[focus]);

  return (
    <div className='input-field'>
    <label
    htmlFor={labelText}
    className="label"
    >
    {labelText}
    </label>
    <input
      ref={inputRef}
      className="required-field text-regular-m"
      type={type ?? 'text'}
      id={labelText}
      value={value}
      onChange={onChange}
      readOnly={readonly}
      required={isRequired}
      placeholder={placeholder}
    />
  </div>
  );
};