import React from 'react';
import { Link } from 'react-router-dom';
import { useEnrollments, useEnrollmentOperations } from '../hooks/useEnrollments';
import { useCourses } from '../hooks/useCourses';
import { useStudents } from '../hooks/useStudents';
import './Enrollments.css';

const Enrollments: React.FC = () => {
  // Custom hooks
  const { enrollments, isLoading: enrollmentsLoading, error: enrollmentsError, refetch: refetchEnrollments } = useEnrollments();
  const { courses, isLoading: coursesLoading } = useCourses();
  const { students, isLoading: studentsLoading } = useStudents();
  const { deleteEnrollment, isDeleting } = useEnrollmentOperations();

  const loading = enrollmentsLoading || coursesLoading || studentsLoading;
  const error = enrollmentsError;

  const handleDeleteEnrollment = async (enrollmentId: number) => {
    if (!window.confirm('Are you sure you want to delete this enrollment?')) {
      return;
    }

    try {
      await deleteEnrollment(enrollmentId);
      await refetchEnrollments(); // Refresh the enrollments list
    } catch (error) {
      console.error('Error deleting enrollment:', error);
      // Error handling is done by the custom hook with toast notifications
    }
  };

  const getCourseName = (courseId: number) => {
    const course = courses.find(c => c.id === courseId);
    return course ? course.name : `Course #${courseId}`;
  };

  const getStudentName = (studentId: number) => {
    const student = students.find(s => s.id === studentId);
    return student ? student.fullName : `Student #${studentId}`;
  };

  if (loading) {
    return (
      <div className="enrollments-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading enrollments...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="enrollments-page">
        <div className="error-message">
          <h3>Error Loading Enrollments</h3>
          <p>{error}</p>
          <button onClick={refetchEnrollments} className="retry-button">
            Retry
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="enrollments-page">
      <div className="page-header">
        <h1>Enrollments</h1>
        <Link to="/enrollments/new" className="primary-button">
          + New Enrollment
        </Link>
      </div>

      {enrollments.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">📝</div>
          <h3>No enrollments found</h3>
          <p>Start by enrolling students in courses to track their progress.</p>
          <Link to="/enrollments/new" className="primary-button">
            Create First Enrollment
          </Link>
        </div>
      ) : (
        <div className="enrollments-container">
          <div className="enrollments-stats">
            <div className="stat-item">
              <span className="stat-number">{enrollments.length}</span>
              <span className="stat-label">Total Enrollments</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">
                {enrollments.filter(e => e.status === 'Active').length}
              </span>
              <span className="stat-label">Active</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">
                {enrollments.filter(e => e.status === 'Completed').length}
              </span>
              <span className="stat-label">Completed</span>
            </div>
            <div className="stat-item">
              <span className="stat-number">
                {enrollments.filter(e => e.status === 'Dropped').length}
              </span>
              <span className="stat-label">Dropped</span>
            </div>
          </div>

          <div className="enrollments-table-container">
            <table className="enrollments-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Student</th>
                  <th>Course</th>
                  <th>Status</th>
                  <th>Enrollment Date</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {enrollments.map((enrollment) => (
                  <tr key={enrollment.id}>
                    <td>{enrollment.id}</td>
                    <td>
                      <div className="student-info">
                        <strong>{getStudentName(enrollment.studentId)}</strong>
                        <small>ID: {enrollment.studentId}</small>
                      </div>
                    </td>
                    <td>
                      <div className="course-info">
                        <strong>{getCourseName(enrollment.courseId)}</strong>
                        <small>ID: {enrollment.courseId}</small>
                      </div>
                    </td>
                    <td>
                      <span className={`status ${enrollment.status.toLowerCase()}`}>
                        {enrollment.status}
                      </span>
                    </td>
                    <td>
                      {new Date(enrollment.enrollmentDate).toLocaleDateString()}
                    </td>
                    <td>
                      <div className="action-buttons">
                        <Link 
                          to={`/enrollments/${enrollment.id}`} 
                          className="action-button view-button"
                        >
                          View
                        </Link>
                        <button 
                          onClick={() => handleDeleteEnrollment(enrollment.id)}
                          className="action-button delete-button"
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default Enrollments;
