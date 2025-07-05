import React, { useState, useEffect } from 'react';
import { EnrollmentReport, LoadingState } from '../types';
import api from '../services/api';
import './Reports.css';

const Reports: React.FC = () => {
  const [reportData, setReportData] = useState<EnrollmentReport | null>(null);
  const [loading, setLoading] = useState<LoadingState>({
    isLoading: true,
    error: null,
  });

  useEffect(() => {
    fetchReportData();
  }, []);

  const fetchReportData = async () => {
    try {
      setLoading({ isLoading: true, error: null });
      const data = await api.enrollments.getReport();
      setReportData(data);
    } catch (error) {
      console.error('Error fetching report data:', error);
      setLoading({ 
        isLoading: false, 
        error: 'Failed to load report data. Please try again.' 
      });
      return;
    }
    
    setLoading({ isLoading: false, error: null });
  };

  const getUtilizationColor = (percentage: number) => {
    if (percentage >= 80) return '#dc3545'; // Red for high utilization
    if (percentage >= 60) return '#ffc107'; // Yellow for medium utilization
    return '#28a745'; // Green for low utilization
  };

  const getUtilizationLabel = (percentage: number) => {
    if (percentage >= 80) return 'High';
    if (percentage >= 60) return 'Medium';
    return 'Low';
  };

  if (loading.isLoading) {
    return (
      <div className="reports-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading report data...</p>
        </div>
      </div>
    );
  }

  if (loading.error) {
    return (
      <div className="reports-page">
        <div className="error-message">
          <h3>Error Loading Reports</h3>
          <p>{loading.error}</p>
          <button onClick={fetchReportData} className="retry-button">
            Retry
          </button>
        </div>
      </div>
    );
  }

  if (!reportData) {
    return (
      <div className="reports-page">
        <div className="empty-state">
          <h3>No Report Data Available</h3>
          <p>There's no enrollment data to generate reports.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="reports-page">
      <div className="reports-header">
        <h1>Enrollment Reports</h1>
        <button onClick={fetchReportData} className="refresh-button">
          🔄 Refresh Data
        </button>
      </div>

      <div className="reports-summary">
        <div className="summary-card">
          <div className="summary-icon">📊</div>
          <div className="summary-info">
            <h3>{reportData.totalEnrollments}</h3>
            <p>Total Enrollments</p>
          </div>
        </div>
        
        <div className="summary-card">
          <div className="summary-icon">📚</div>
          <div className="summary-info">
            <h3>{reportData.courseEnrollments.length}</h3>
            <p>Courses with Enrollments</p>
          </div>
        </div>
        
        <div className="summary-card">
          <div className="summary-icon">👥</div>
          <div className="summary-info">
            <h3>{reportData.studentEnrollments.length}</h3>
            <p>Students Enrolled</p>
          </div>
        </div>
        
        <div className="summary-card">
          <div className="summary-icon">📈</div>
          <div className="summary-info">
            <h3>
              {reportData.courseEnrollments.length > 0 
                ? Math.round(reportData.courseEnrollments.reduce((sum, c) => sum + c.utilizationPercentage, 0) / reportData.courseEnrollments.length)
                : 0}%
            </h3>
            <p>Avg. Course Utilization</p>
          </div>
        </div>
      </div>

      <div className="reports-content">
        <div className="report-section">
          <h2>Course Enrollment Analysis</h2>
          
          {reportData.courseEnrollments.length === 0 ? (
            <div className="empty-section">
              <p>No course enrollments found.</p>
            </div>
          ) : (
            <div className="course-enrollments-table">
              <table>
                <thead>
                  <tr>
                    <th>Course</th>
                    <th>Enrolled</th>
                    <th>Capacity</th>
                    <th>Utilization</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {reportData.courseEnrollments.map((course, index) => (
                    <tr key={index}>
                      <td>
                        <div className="course-name">
                          <strong>{course.courseName}</strong>
                          <small>ID: {course.courseId}</small>
                        </div>
                      </td>
                      <td>
                        <span className="enrollment-count">
                          {course.enrollmentCount}
                        </span>
                      </td>
                      <td>
                        <span className="capacity-count">
                          {course.capacity}
                        </span>
                      </td>
                      <td>
                        <div className="utilization-bar">
                          <div 
                            className="utilization-fill" 
                            style={{ 
                              width: `${Math.min(course.utilizationPercentage, 100)}%`,
                              backgroundColor: getUtilizationColor(course.utilizationPercentage)
                            }}
                          />
                          <span className="utilization-text">
                            {course.utilizationPercentage.toFixed(1)}%
                          </span>
                        </div>
                      </td>
                      <td>
                        <span 
                          className={`utilization-status ${getUtilizationLabel(course.utilizationPercentage).toLowerCase()}`}
                        >
                          {getUtilizationLabel(course.utilizationPercentage)}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>

        <div className="report-section">
          <h2>Student Enrollment Summary</h2>
          
          {reportData.studentEnrollments.length === 0 ? (
            <div className="empty-section">
              <p>No student enrollments found.</p>
            </div>
          ) : (
            <div className="student-enrollments-grid">
              {reportData.studentEnrollments.map((student, index) => (
                <div key={index} className="student-card">
                  <div className="student-header">
                    <h4>{student.studentName}</h4>
                    <span className="course-count">
                      {student.courseCount} course{student.courseCount !== 1 ? 's' : ''}
                    </span>
                  </div>
                  <div className="student-courses">
                    {student.courses.map((course, courseIndex) => (
                      <span key={courseIndex} className="course-tag">
                        {course}
                      </span>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Reports;
