import Navbar from "./components/Navbar/navbar";
import { BrowserRouter as Router, Routes, Route, Outlet } from 'react-router-dom';
import Footer from "./components/Footer/Footer";
function Layout() {
    return (
      <div className="layout" style={{width: "100%", height: "100%"}}>
        <header>
          <Navbar />
        </header>
          <Outlet />
      <Footer />
      </div>
    );
  }

export default Layout;