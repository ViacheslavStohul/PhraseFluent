import React from 'react';
import { ReactElement } from 'react';

const PlusSVG = (): ReactElement => {
  return (
<svg
          xmlns="http://www.w3.org/2000/svg"
          height='20'
          width='20'
          viewBox="0 0 40 40"
          fill="none"
          stroke='currentColor'
        >
          <line
          x1='20' 
          y1='5' 
          x2='20' 
          y2='35' 
          strokeWidth={5}
          strokeLinecap="round"></line>
          <line
          x1='5' 
          y1='20' 
          x2='35' 
          y2='20' 
          strokeWidth={5}
          strokeLinecap="round"></line>
        </svg>
  );
};

export default PlusSVG;


