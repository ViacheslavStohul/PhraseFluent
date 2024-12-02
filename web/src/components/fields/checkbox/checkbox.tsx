import React from 'react';
import './checkbox.scss';

interface CustomCheckboxProps {
  label: string;
  checked?: boolean;
  onChange: (checked: boolean) => void;
  isRadio?: boolean;
}

const Checkbox: React.FC<CustomCheckboxProps> = ({
  label,
  checked = false,
  onChange,
  isRadio = false,
}) => {


  return (
    <label className="custom-checkbox">
      <input
        type="checkbox"
        checked={checked ?? false}
        onChange={()=> onChange(!checked)}
        className="custom-checkbox-input"
      />
      <span className={`custom-checkbox-marker ${isRadio ? 'radio' : 'check'}`} />
      {label && <span className="custom-checkbox-label">{label}</span>}
    </label>
  );
};

export default Checkbox;
