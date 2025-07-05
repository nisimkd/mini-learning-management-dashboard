import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import './Layout.css';

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const location = useLocation();

  const isActiveRoute = (path: string) => {
    return location.pathname === path;
  };

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="navbar-brand">
          <Link to="/" className="brand-link">
            📚 Learning Management Dashboard
          </Link>
        </div>
        <ul className="navbar-nav">
          <li className={`nav-item ${isActiveRoute('/') ? 'active' : ''}`}>
            <Link to="/" className="nav-link">
              Dashboard
            </Link>
          </li>
          <li className={`nav-item ${isActiveRoute('/courses') ? 'active' : ''}`}>
            <Link to="/courses" className="nav-link">
              Courses
            </Link>
          </li>
          <li className={`nav-item ${isActiveRoute('/enrollments') ? 'active' : ''}`}>
            <Link to="/enrollments" className="nav-link">
              Enrollments
            </Link>
          </li>
          <li className={`nav-item ${isActiveRoute('/reports') ? 'active' : ''}`}>
            <Link to="/reports" className="nav-link">
              Reports
            </Link>
          </li>
        </ul>
      </nav>
      <main className="main-content">
        {children}
      </main>
    </div>
  );
};

export default Layout;
