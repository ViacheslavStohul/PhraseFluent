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
            path: '/',
            lazy: () => import('../components/pages/home/homepage').then(
              (module) => ({
                Component: module.default
              })
            )
          },
          {
            path: 'profile',
            lazy: () => import('../components/pages/profile/profile').then(
              (module) => ({
                Component: module.default
              })
            )
          },
          {
            path: 'new',
            lazy: () => import('../components/pages/new/new').then(
              (module) => ({
                Component: module.default
              })
            )
          },
          {
            path: 'test',
            lazy: () => import('../components/pages/test/test').then(
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
    ],
  },
  {
    lazy: () => import('../components/layouts/form-card/form-card').then(
      (module) => ({
        Component: module.default
      })),
    children: [
      {
        path: 'authorization',
        lazy: () => import('../components/pages/authorization/authorization').then(
          (module) => ({
            Component: module.default
          })
        )
      },
      {
        path: 'registration',
        lazy: () => import('../components/pages/registration/registration').then(
          (module) => ({
            Component: module.default
          })
        )
      },
      {
        path: '*',
        lazy: () => import('../components/pages/authorization/authorization').then(
          (module) => ({
            Component: module.default
          })
        ),
      }
    ]
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
