import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { CourseDto, Enrollment, Student, LoadingState } from '../types';
import api from '../services/api';
import './Dashboard.css';

const Dashboard: React.FC = () => {
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState<LoadingState>({
    isLoading: true,
    error: null,
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading({ isLoading: true, error: null });
        
        const [coursesData, enrollmentsData, studentsData] = await Promise.all([
          api.courses.getAll(),
          api.enrollments.getAll(),
          api.students.getAll(),
        ]);
        
        setCourses(coursesData);
        setEnrollments(enrollmentsData);
        setStudents(studentsData);
      } catch (error) {
        console.error('Error fetching dashboard data:', error);
        setLoading({ 
          isLoading: false, 
          error: 'Failed to load dashboard data. Please try again.' 
        });
        return;
      }
      
      setLoading({ isLoading: false, error: null });
    };

    fetchData();
  }, []);

  const activeCourses = courses.filter(c => c.isActive);
  const activeStudents = students.filter(s => s.isActive);
  const recentEnrollments = enrollments
    .sort((a, b) => new Date(b.enrollmentDate).getTime() - new Date(a.enrollmentDate).getTime())
    .slice(0, 5);

  if (loading.isLoading) {
    return (
      <div className="dashboard">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading dashboard...</p>
        </div>
      </div>
    );
  }

  if (loading.error) {
    return (
      <div className="dashboard">
        <div className="error-message">
          <h3>Error Loading Dashboard</h3>
          <p>{loading.error}</p>
          <button onClick={() => window.location.reload()} className="retry-button">
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="dashboard">
      <div className="dashboard-header">
        <h1>Learning Management Dashboard</h1>
        <p>Welcome to your education management system</p>
      </div>

      <div className="dashboard-stats">
        <div className="stat-card">
          <div className="stat-icon">📚</div>
          <div className="stat-info">
            <h3>{activeCourses.length}</h3>
            <p>Active Courses</p>
          </div>
        </div>
        
        <div className="stat-card">
          <div className="stat-icon">👥</div>
          <div className="stat-info">
            <h3>{activeStudents.length}</h3>
            <p>Active Students</p>
          </div>
        </div>
        
        <div className="stat-card">
          <div className="stat-icon">📝</div>
          <div className="stat-info">
            <h3>{enrollments.length}</h3>
            <p>Total Enrollments</p>
          </div>
        </div>
        
        <div className="stat-card">
          <div className="stat-icon">📊</div>
          <div className="stat-info">
            <h3>{Math.round((enrollments.length / activeCourses.length) * 100) / 100 || 0}</h3>
            <p>Avg. Enrollments per Course</p>
          </div>
        </div>
      </div>

      <div className="dashboard-content">
        <div className="dashboard-section">
          <div className="section-header">
            <h2>Recent Enrollments</h2>
            <Link to="/enrollments" className="view-all-link">
              View All →
            </Link>
          </div>
          
          {recentEnrollments.length === 0 ? (
            <div className="empty-state">
              <p>No enrollments yet.</p>
              <Link to="/enrollments" className="primary-button">
                Create First Enrollment
              </Link>
            </div>
          ) : (
            <div className="recent-enrollments">
              {recentEnrollments.map((enrollment) => (
                <div key={enrollment.id} className="enrollment-item">
                  <div className="enrollment-info">
                    <h4>Student #{enrollment.studentId}</h4>
                    <p>Course #{enrollment.courseId}</p>
                    <span className={`status ${enrollment.status.toLowerCase()}`}>
                      {enrollment.status}
                    </span>
                  </div>
                  <div className="enrollment-date">
                    {new Date(enrollment.enrollmentDate).toLocaleDateString()}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        <div className="dashboard-section">
          <div className="section-header">
            <h2>Quick Actions</h2>
          </div>
          
          <div className="quick-actions">
            <Link to="/courses/new" className="action-card">
              <div className="action-icon">➕</div>
              <h3>Add New Course</h3>
              <p>Create a new course for students</p>
            </Link>
            
            <Link to="/enrollments/new" className="action-card">
              <div className="action-icon">👤</div>
              <h3>Enroll Student</h3>
              <p>Assign a student to a course</p>
            </Link>
            
            <Link to="/reports" className="action-card">
              <div className="action-icon">📈</div>
              <h3>View Reports</h3>
              <p>Generate enrollment analytics</p>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
