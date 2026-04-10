import React, { useState, useEffect } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import StudentList from './components/StudentList';
import StudentForm from './components/StudentForm';
import Navbar from './components/Navbar';
import './App.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [token, setToken] = useState('');

  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      setToken(storedToken);
      setIsAuthenticated(true);
    }
  }, []);

  const handleLogin = (token) => {
    localStorage.setItem('token', token);
    setToken(token);
    setIsAuthenticated(true);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken('');
    setIsAuthenticated(false);
  };

  return (
    <div className="App">
      {isAuthenticated && <Navbar onLogout={handleLogout} />}
      <div className="container">
        <Routes>
          <Route 
            path="/login" 
            element={!isAuthenticated ? <Login onLogin={handleLogin} /> : <Navigate to="/students" />} 
          />
          <Route 
            path="/students" 
            element={isAuthenticated ? <StudentList token={token} /> : <Navigate to="/login" />} 
          />
          <Route 
            path="/students/new" 
            element={isAuthenticated ? <StudentForm token={token} /> : <Navigate to="/login" />} 
          />
          <Route 
            path="/students/edit/:id" 
            element={isAuthenticated ? <StudentForm token={token} /> : <Navigate to="/login" />} 
          />
          <Route 
            path="/" 
            element={<Navigate to={isAuthenticated ? "/students" : "/login"} />} 
          />
        </Routes>
      </div>
    </div>
  );
}

export default App;
