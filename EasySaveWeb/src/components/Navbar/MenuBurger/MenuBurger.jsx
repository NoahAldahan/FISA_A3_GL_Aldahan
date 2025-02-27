import React, { useState } from 'react';
import './menuBurger.css';
import { NavLink } from 'react-router-dom';


function handleClick(setBarresState, navBarBurgerState, setNavBarBurgerState)
{
    console.log("navbar", navBarBurgerState)
    if(navBarBurgerState == "navbarBurger-close"){
        setBarresState("barre-open")
        setNavBarBurgerState("navbarBurger-open")
    }
    else{
        setBarresState("barre-close");
        setNavBarBurgerState("navbarBurger-close");
    }
}
function MenuBurger() {
    const [barresState, setBarresState] = useState("barre-close")
    const [navBarBurgerState, setNavBarBurgerState] = useState("navbarBurger-close")
    return (<>
    <div id="menuBurger" onClick={() => {handleClick(setBarresState, navBarBurgerState, setNavBarBurgerState)}} className="menuBurger">
        <div id="barre" className={barresState}></div>
        <div id="barre" className={barresState}></div>
        <div id="barre" className={barresState}></div>
    </div>
    <div className={navBarBurgerState} id="navbarBurger">
        <NavLink href="/index.html">Acceuil</NavLink>
        <NavLink href="/index.html">Générer vidéo</NavLink>
        <NavLink href="/index.html">Contact</NavLink>
        <NavLink href="/index.html">Services</NavLink>
    </div>
    </>);
}

export default MenuBurger;