import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Student, Course, Enrollment, LoadingState } from '../types';
import { studentApi, courseApi, enrollmentApi } from '../services/api';
import './Students.css';

const Students: React.FC = () => {
  const [students, setStudents] = useState<Student[]>([]);
  const [courses, setCourses] = useState<Course[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState<LoadingState>({
    isLoading: true,
    error: null,
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading({ isLoading: true, error: null });
      
      const [studentsData, coursesData, enrollmentsData] = await Promise.all([
        studentApi.getAll(),
        courseApi.getAll(),
        enrollmentApi.getAll(),
      ]);
      
      setStudents(studentsData);
      setCourses(coursesData);
      setEnrollments(enrollmentsData);
    } catch (error) {
      console.error('Error fetching data:', error);
      setLoading({ 
        isLoading: false, 
        error: 'Failed to load students data. Please try again.' 
      });
      return;
    }
    
    setLoading({ isLoading: false, error: null });
  };

  const getStudentEnrollments = (studentId: number) => {
    return enrollments.filter(e => e.studentId === studentId);
  };

  const getEnrolledCourses = (studentId: number) => {
    const studentEnrollments = getStudentEnrollments(studentId);
    return studentEnrollments.map(enrollment => {
      const course = courses.find(c => c.id === enrollment.courseId);
      return course ? course.name : `Course #${enrollment.courseId}`;
    });
  };

  const filteredStudents = students.filter(student =>
    student.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    student.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading.isLoading) {
    return (
      <div className="students-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading students...</p>
        </div>
      </div>
    );
  }

  if (loading.error) {
    return (
      <div className="students-page">
        <div className="error-message">
          <h3>Error Loading Students</h3>
          <p>{loading.error}</p>
          <button onClick={fetchData} className="retry-button">
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="students-page">
      <div className="page-header">
        <h1>Students</h1>
        <div className="header-actions">
          <Link to="/enrollments/new" className="primary-button">
            + New Enrollment
          </Link>
        </div>
      </div>

      <div className="students-stats">
        <div className="stat-card">
          <div className="stat-icon">👥</div>
          <div className="stat-info">
            <h3>{students.length}</h3>
            <p>Total Students</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">📚</div>
          <div className="stat-info">
            <h3>{courses.length}</h3>
            <p>Available Courses</p>
          </div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">📝</div>
          <div className="stat-info">
            <h3>{enrollments.length}</h3>
            <p>Total Enrollments</p>
          </div>
        </div>
      </div>

      <div className="search-section">
        <input
          type="text"
          placeholder="Search students by name or email..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="search-input"
        />
      </div>

      {filteredStudents.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">👥</div>
          <h3>No students found</h3>
          <p>
            {searchTerm 
              ? `No students match "${searchTerm}". Try a different search term.`
              : 'No students are currently registered in the system.'
            }
          </p>
        </div>
      ) : (
        <div className="students-grid">
          {filteredStudents.map((student) => {
            const enrolledCourses = getEnrolledCourses(student.id);
            return (
              <div key={student.id} className="student-card">
                <div className="student-header">
                  <div className="student-avatar">
                    {student.fullName.charAt(0).toUpperCase()}
                  </div>
                  <div className="student-info">
                    <h3>{student.fullName}</h3>
                    <p className="student-email">{student.email}</p>
                    <p className="student-id">ID: {student.id}</p>
                  </div>
                </div>
                
                <div className="student-enrollments">
                  <h4>Enrolled Courses ({enrolledCourses.length})</h4>
                  {enrolledCourses.length === 0 ? (
                    <p className="no-enrollments">No course enrollments</p>
                  ) : (
                    <ul className="course-list">
                      {enrolledCourses.map((courseName, index) => (
                        <li key={index} className="course-item">
                          {courseName}
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
                
                <div className="student-actions">
                  <Link 
                    to={`/enrollments/new?studentId=${student.id}`}
                    className="action-button primary"
                  >
                    Enroll in Course
                  </Link>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default Students;
