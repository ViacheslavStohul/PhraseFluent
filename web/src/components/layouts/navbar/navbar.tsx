import React from 'react';
import './navbar.scss';
import { NavLink } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';
import LogoSVG from '../../svg/logo';
import User from './user/user';

const Navbar = () => {
  const user = useSelector(AuthSelectors.selectUsername);
  const dispatch = useDispatch();

  const logout = () => {
    dispatch(AuthActions.logout());
  }

  return (
    <nav>
      <div><LogoSVG/></div>
      <div className='nav-block'>
        { user ? 
          <>
          <User/>
          <div onClick={logout} className='pointer'>Log out</div>
          </>
          :
          <>
          <NavLink to='/registration' end>Sign up</NavLink>
          <NavLink to='/authorization' end>Sign in</NavLink>
          </>
        }
      </div>
    </nav>
  );
}

export default Navbar;
