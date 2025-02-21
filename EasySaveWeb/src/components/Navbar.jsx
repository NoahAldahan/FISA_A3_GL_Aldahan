import { useState } from "react";
import "./Navbar.css"; // Importation du fichier CSS

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <nav className="navbar">
      <div className="navbar-container">
        {/* Logo */}
        <a href="#" className="logo">
          MyBrand
        </a>

        {/* Menu Desktop */}
        <ul className={`nav-links ${isOpen ? "open" : ""}`}>
          <li><a href="#">Accueil</a></li>
          <li><a href="#">Services</a></li>
          <li><a href="#">Contact</a></li>
        </ul>

        {/* Bouton Burger Mobile */}
        <button className="burger" onClick={() => setIsOpen(!isOpen)}>
          <div className={`line ${isOpen ? "rotate1" : ""}`}></div>
          <div className={`line ${isOpen ? "hide" : ""}`}></div>
          <div className={`line ${isOpen ? "rotate2" : ""}`}></div>
        </button>
      </div>
    </nav>
  );
}