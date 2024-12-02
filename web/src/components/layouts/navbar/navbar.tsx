import React from 'react';
import './navbar.scss';
import { Link, NavLink } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { AuthActions, AuthSelectors } from '../../../store/slice/auth';

const Navbar = () => {
  const user = useSelector(AuthSelectors.selectUser);
  const dispatch = useDispatch();

  const logout = () => {
    dispatch(AuthActions.logout());
  }

  return (
    <nav>
      <div className="logo">
            <img src="/logo.png" alt="Logo"/>
        </div>
        <div className="logo logo-two">
            <Link to="/">
            <img src="/logo-2.png" alt="Logo"/>
            </Link>
        </div>
        <div className="nav-links">
            <NavLink to="/">Головна</NavLink>
            <NavLink to="/tests">Опитування</NavLink>
            { user ? 
              <div onClick={logout} className='pointer exit'>Вийти</div>
            :
            <>
            <NavLink to='/authorization' end>Вхід</NavLink>
            </>
            }
        </div>
    </nav>
  );
}

export default Navbar;
