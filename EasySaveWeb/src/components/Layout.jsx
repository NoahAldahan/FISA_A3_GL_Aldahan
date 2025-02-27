import Navbar from "./Navbar/Navbar";
import { BrowserRouter as Router, Routes, Route, Outlet } from 'react-router-dom';
// import Footer from "./components/Footer/Footer";
function Layout() {
    return (
      <div className="layout" style={{width: "100%", height: "100%"}}>
          <Navbar />
          <Outlet />
      {/* <Footer /> */}
      </div>
    );
  }

export default Layout;