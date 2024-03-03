import React from 'react';
import './navbar.scss';
import { NavLink } from 'react-router-dom';
function Navbar() {
  return (
    <nav>
      <div>Logo</div>
      <NavLink to='/authorization' end>Sign in</NavLink>
    </nav>
  );
}

export default Navbar;
