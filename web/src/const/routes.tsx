import { RouteObject, createBrowserRouter } from 'react-router-dom';
import React from 'react';
import App from '../App';

const children: RouteObject[] = [
  {
    path: '',
    lazy: () =>
      import('../components/layouts/main-layout/main-layout').then(
        (module) => ({
          Component: module.default
        })
      ),
    children: []
  }
];

export const routes = createBrowserRouter([
  {
    path: '',
    element: <App />,
    children
  }
]);
