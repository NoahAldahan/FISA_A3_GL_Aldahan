import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import React  from 'react';
import { BrowserRouter as Router, Routes, Route, createBrowserRouter, Navigate, RouterProvider } from 'react-router-dom';
import ReactDOM from 'react-dom/client';
import './index.css';
import Layout from './components/Layout.jsx';
import Home from './routes/Home/Home.jsx'
import SaveTask from './routes/SaveTask/SaveTask.jsx';


const root = ReactDOM.createRoot(document.getElementById('root'));
const router = createBrowserRouter(
  [
    {
      path: '/',
      element: <Layout />,
      children:[
        {
          path: '/',
          element: <Home />,
        },
        {
          path: '/saveTask',
          element: <SaveTask />,
        },
        {
          path: '/about',
          element: <Home />,
        }
      ]
    },
    {
      path: "*",
      element: <Navigate to="/" />
    }
  ]
)
ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>);