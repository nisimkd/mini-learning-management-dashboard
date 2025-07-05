import React, { useState, useEffect } from 'react';
import { EnrollmentReport } from '../types';
import { studentApi } from '../services/api';
import './Reports.css';

const Reports: React.FC = () => {
  const [reportData, setReportData] = useState<EnrollmentReport | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchReportData = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await studentApi.getEnrollmentReport();
        setReportData(data);
      } catch (error) {
        console.error('Error fetching report data:', error);
        setError('Failed to load report data. Please try again.');
      } finally {
        setLoading(false);
      }
    };

    fetchReportData();
  }, []);

  if (loading) {
    return (
      <div className="reports-container">
        <div className="loading-state">
          <div className="loading-spinner"></div>
          <p>Loading enrollment report...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="reports-container">
        <div className="error-state">
          <h2>Error</h2>
          <p>{error}</p>
          <button onClick={() => window.location.reload()}>
            Try Again
          </button>
        </div>
      </div>
    );
  }

  if (!reportData) {
    return (
      <div className="reports-container">
        <div className="empty-state">
          <h2>No Data Available</h2>
          <p>No enrollment data found.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="reports-container">
      <header className="reports-header">
        <h1>📊 Enrollment Report</h1>
        <p>Summary of students, courses, and enrollments</p>
      </header>

      <div className="report-summary">
        <div className="summary-card">
          <div className="summary-icon">🧑‍🎓</div>
          <div className="summary-content">
            <h3>{reportData.totalStudents}</h3>
            <p>Total Students</p>
          </div>
        </div>

        <div className="summary-card">
          <div className="summary-icon">📚</div>
          <div className="summary-content">
            <h3>{reportData.totalCourses}</h3>
            <p>Total Courses</p>
          </div>
        </div>

        <div className="summary-card">
          <div className="summary-icon">✅</div>
          <div className="summary-content">
            <h3>{reportData.courseEnrollments.reduce((sum, course) => sum + course.enrolledStudentsCount, 0)}</h3>
            <p>Total Enrollments</p>
          </div>
        </div>
      </div>

      <div className="course-enrollments-section">
        <h2>Course Enrollment Details</h2>
        
        {reportData.courseEnrollments.length === 0 ? (
          <div className="empty-state">
            <p>No courses found.</p>
          </div>
        ) : (
          <div className="course-enrollments-grid">
            {reportData.courseEnrollments.map((course, index) => (
              <div key={index} className="course-enrollment-card">
                <div className="course-header">
                  <h3>{course.courseName}</h3>
                </div>
                
                <div className="enrollment-stats">
                  <div className="stat">
                    <span className="stat-label">Enrolled Students:</span>
                    <span className="stat-value">{course.enrolledStudentsCount}</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Reports;
