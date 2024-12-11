import React, { ChangeEvent, } from 'react';
import './input-field.scss';

export interface IInputFieldProps {
  labelText: string;
  value?: string | number;
  changed?: (value: string) => void;
  readonly?: boolean;
  type?: string;
  isRequired?: boolean;
  placeholder?: string;
  disabled?: boolean;
  name: string;
  maxLength?: number;
}

export const InputFieldComponent = (
  props: IInputFieldProps
): React.JSX.Element => {
  const { labelText, value, changed, readonly, type, isRequired, placeholder, name, disabled, maxLength } =
    props;

  const onChange = (event: ChangeEvent<HTMLInputElement>) => {
    if (changed){
        changed(event.target.value);
    }
  };

  return (
    <div className='input-field'>
    <label
    htmlFor={labelText}
    className="label"
    >
    {labelText}{isRequired && '*'}
    </label>
    <input
      maxLength={maxLength}
      className="text-regular-m"
      type={type ?? 'text'}
      id={labelText}
      value={value}
      name={name}
      disabled={disabled}
      onChange={onChange}
      readOnly={readonly}
      required={isRequired}
      placeholder={placeholder}
    />
  </div>
  );
};