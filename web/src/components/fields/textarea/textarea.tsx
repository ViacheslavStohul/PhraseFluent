import React, { ChangeEvent, useEffect, useRef } from 'react';
import './textarea.scss';

export interface ITextareaFieldProps {
  labelText: string;
  value?: string;
  changed?: (value: string) => void;
  readonly?: boolean;
  isRequired?: boolean;
  placeholder?: string;
  regex?: RegExp;
  focus?: boolean;
  name: string;
  rows?: number;
  cols?: number;
}

export const TextareaFieldComponent = (
  props: ITextareaFieldProps
): React.JSX.Element => {
  const {
    labelText,
    value,
    changed,
    readonly,
    isRequired,
    placeholder,
    regex,
    focus,
    name,
    rows,
    cols,
  } = props;
  const textareaRef = useRef<HTMLTextAreaElement | null>(null);

  const onChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
    if (changed) {
      if (regex?.test(event.target.value) || !regex) {
        changed(event.target.value);
      }
    }
  };

  useEffect(() => {
    if (focus && textareaRef.current) {
      textareaRef.current.focus();
    } else if (textareaRef.current) {
      textareaRef.current.blur();
    }
  }, [focus]);

  return (
    <div className="textarea-field">
      <label htmlFor={labelText} className="label">
        {labelText}
        {isRequired && '*'}
      </label>
      <textarea
        maxLength={200}
        ref={textareaRef}
        className="required-field text-regular-m"
        id={labelText}
        value={value}
        name={name}
        onChange={onChange}
        readOnly={readonly}
        required={isRequired}
        placeholder={placeholder}
        rows={rows ?? 4}
        cols={cols ?? 50}
      />
    </div>
  );
};