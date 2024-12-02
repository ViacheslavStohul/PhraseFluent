import React, { useState } from 'react';
import './checkbox.scss';

interface CustomCheckboxProps {
  label: string;
  checked?: boolean;
  onChange?: (checked: boolean) => void;
}

const Checkbox: React.FC<CustomCheckboxProps> = ({
  label,
  checked = false,
  onChange,
}) => {
  const [isChecked, setIsChecked] = useState(checked);

  const handleCheckboxChange = () => {
    const newCheckedState = !isChecked;
    setIsChecked(newCheckedState);
    if (onChange) {
      onChange(newCheckedState);
    }
  };

  return (
    <label className="custom-checkbox">
      <input
        type="checkbox"
        checked={isChecked}
        onChange={handleCheckboxChange}
        className="custom-checkbox-input"
      />
      <span className="custom-checkbox-marker" />
      {label && <span className="custom-checkbox-label">{label}</span>}
    </label>
  );
};

export default Checkbox;
