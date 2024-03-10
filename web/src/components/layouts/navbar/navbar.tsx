import React from 'react';
import './navbar.scss';
import { NavLink } from 'react-router-dom';

const Navbar = () => {
  return (
    <nav>
      <div>Logo</div>
      <div className='nav-block'>
      <NavLink to='/registration' end>Sign up</NavLink>
      <NavLink to='/authorization' end>Sign in</NavLink>
      </div>
    </nav>
  );
}

export default Navbar;
