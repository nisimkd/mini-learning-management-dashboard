import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/layout/Layout';
import Dashboard from './pages/Dashboard';
import Courses from './pages/Courses';
import Enrollments from './pages/Enrollments';
import './App.css';

function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/courses" element={<Courses />} />
          <Route path="/enrollments" element={<Enrollments />} />
          <Route path="/reports" element={<div>Reports - Coming Soon</div>} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;
