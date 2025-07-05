import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { CourseDto, LoadingState } from '../types';
import api from '../services/api';
import './Courses.css';

const Courses: React.FC = () => {
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [loading, setLoading] = useState<LoadingState>({
    isLoading: true,
    error: null,
  });

  useEffect(() => {
    fetchCourses();
  }, []);

  const fetchCourses = async () => {
    try {
      setLoading({ isLoading: true, error: null });
      const coursesData = await api.courses.getAll();
      setCourses(coursesData);
    } catch (error) {
      console.error('Error fetching courses:', error);
      setLoading({ 
        isLoading: false, 
        error: 'Failed to load courses. Please try again.' 
      });
      return;
    }
    
    setLoading({ isLoading: false, error: null });
  };

  const handleDeleteCourse = async (courseId: number) => {
    if (!window.confirm('Are you sure you want to delete this course?')) {
      return;
    }

    try {
      await api.courses.delete(courseId);
      setCourses(courses.filter(c => c.id !== courseId));
    } catch (error) {
      console.error('Error deleting course:', error);
      alert('Failed to delete course. Please try again.');
    }
  };

  if (loading.isLoading) {
    return (
      <div className="courses-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading courses...</p>
        </div>
      </div>
    );
  }

  if (loading.error) {
    return (
      <div className="courses-page">
        <div className="error-message">
          <h3>Error Loading Courses</h3>
          <p>{loading.error}</p>
          <button onClick={fetchCourses} className="retry-button">
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="courses-page">
      <div className="page-header">
        <h1>Courses</h1>
        <Link to="/courses/new" className="primary-button">
          + Add New Course
        </Link>
      </div>

      {courses.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">📚</div>
          <h3>No courses available</h3>
          <p>Start by creating your first course to get students enrolled.</p>
          <Link to="/courses/new" className="primary-button">
            Create First Course
          </Link>
        </div>
      ) : (
        <div className="courses-grid">
          {courses.map((course) => (
            <div key={course.id} className="course-card">
              <div className="course-header">
                <h3>{course.title}</h3>
                <span className={`status ${course.isActive ? 'active' : 'inactive'}`}>
                  {course.isActive ? 'Active' : 'Inactive'}
                </span>
              </div>
              
              <div className="course-info">
                <p className="course-description">{course.description}</p>
                
                <div className="course-details">
                  <div className="detail-item">
                    <span className="label">Code:</span>
                    <span className="value">{course.code}</span>
                  </div>
                  
                  <div className="detail-item">
                    <span className="label">Max Capacity:</span>
                    <span className="value">{course.maxCapacity} students</span>
                  </div>
                  
                  <div className="detail-item">
                    <span className="label">Enrolled:</span>
                    <span className="value">{course.enrolledStudents} students</span>
                  </div>
                  
                  <div className="detail-item">
                    <span className="label">Created:</span>
                    <span className="value">
                      {new Date(course.createdAt).toLocaleDateString()}
                    </span>
                  </div>
                </div>
              </div>
              
              <div className="course-actions">
                <Link 
                  to={`/courses/${course.id}`} 
                  className="action-button view-button"
                >
                  View Details
                </Link>
                <Link 
                  to={`/courses/${course.id}/edit`} 
                  className="action-button edit-button"
                >
                  Edit
                </Link>
                <button 
                  onClick={() => handleDeleteCourse(course.id)}
                  className="action-button delete-button"
                >
                  Delete
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Courses;
