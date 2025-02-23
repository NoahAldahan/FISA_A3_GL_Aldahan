import React, { useState } from 'react';
import './Navbar.css';
// import logo from "../../assets/logo/tiktokscriptLogowhite.png"; 
import { NavLink } from 'react-router-dom';
import MenuBurger from "./MenuBurger/MenuBurger"

function Navbar() {
  
  const [page, setPage] = useState("Home");
  return (
    <nav className="navbar">
      {/* <MenuBurger /> */}
      {/* <NavLink to="/"><img className="logo" src={logo} /></NavLink> */}
      <ul className="menu">
        <li><NavLink className={({ isActive }) => isActive ? "active" : ""} to="/">Home</NavLink></li>
        <li><NavLink className={({ isActive }) => isActive ? "active" : ""} to="/saveTask"  >SaveTask</NavLink></li>
        <li><NavLink className={({ isActive }) => isActive ? "active" : ""} to="/about">About</NavLink></li>
      </ul>
    </nav>
  );
}

export default Navbar;