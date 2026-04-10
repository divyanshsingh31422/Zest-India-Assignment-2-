import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Navbar = ({ onLogout }) => {
  const navigate = useNavigate();

  const handleLogout = () => {
    onLogout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="navbar-content">
        <Link to="/students" className="navbar-brand">
          Student Management System
        </Link>
        <div className="navbar-nav">
          <Link to="/students" className="nav-link">
            Students
          </Link>
          <Link to="/students/new" className="nav-link">
            Add Student
          </Link>
          <button className="btn-logout" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
