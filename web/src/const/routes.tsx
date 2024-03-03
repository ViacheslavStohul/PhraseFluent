import { Navigate, RouteObject, createBrowserRouter } from 'react-router-dom';
import React from 'react';
import App from '../App';

const children: RouteObject[] = [
  {
    lazy: () =>
      import('../components/layouts/main-layout/main-layout').then(
        (module) => ({
          Component: module.default
        })
      ),
    children: [
      {
        lazy: () => import('../components/layouts/main-layout/body-card/body-card').then(
          (module) => ({
            Component: module.default
          })),
        children: [
          {
            path: '/',
            lazy: () => import('../components/pages/home/homepage').then(
              (module) => ({
                Component: module.default
              })
            )
          },
          {
            path: '*',
            lazy: () => import('../components/pages/home/homepage').then(
              (module) => ({
                Component: module.default
              })
            ),
          }
        ] 
      }
    ],
  },
  {
    path: '*',
    element: (
      <Navigate to="/" />
    )
  }
];

export const routes = createBrowserRouter([
  {
    path: '',
    element: <App />,
    children
  }
]);
