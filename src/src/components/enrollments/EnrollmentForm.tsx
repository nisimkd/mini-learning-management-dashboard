import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { CreateEnrollmentDto, Course, Student, FormErrors } from '../../types';
import { courseApi, studentApi, enrollmentApi } from '../../services/api';
import './EnrollmentForm.css';

const EnrollmentForm: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [loading, setLoading] = useState(false);
  const [courses, setCourses] = useState<Course[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [errors, setErrors] = useState<FormErrors>({});
  
  // Get pre-selected student ID from URL params
  const urlParams = new URLSearchParams(location.search);
  const preSelectedStudentId = urlParams.get('studentId');
  
  const [formData, setFormData] = useState<CreateEnrollmentDto>({
    studentId: preSelectedStudentId ? parseInt(preSelectedStudentId) : 0,
    courseId: 0
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [coursesData, studentsData] = await Promise.all([
        courseApi.getAll(),
        studentApi.getAll(),
      ]);
      
      setCourses(coursesData);
      setStudents(studentsData);
    } catch (error) {
      console.error('Error fetching data:', error);
      setErrors({ general: 'Failed to load courses and students' });
    } finally {
      setLoading(false);
    }
  };

  const validateForm = (): boolean => {
    const newErrors: FormErrors = {};

    if (!formData.studentId || formData.studentId === 0) {
      newErrors.studentId = 'Please select a student';
    }

    if (!formData.courseId || formData.courseId === 0) {
      newErrors.courseId = 'Please select a course';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    try {
      setLoading(true);
      setErrors({});

      await enrollmentApi.create(formData);
      navigate('/enrollments');
    } catch (error) {
      console.error('Error creating enrollment:', error);
      setErrors({ general: 'Failed to create enrollment. Please try again.' });
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'studentId' || name === 'courseId' ? parseInt(value) || 0 : value
    }));
  };

  const getStudentName = (student: Student): string => {
    return student.fullName;
  };

  const getCourseName = (course: Course): string => {
    return course.name;
  };

  if (loading) {
    return (
      <div className="enrollment-form-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading form data...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="enrollment-form-page">
      <div className="form-header">
        <h1>Create New Enrollment</h1>
        <button 
          onClick={() => navigate('/enrollments')} 
          className="back-button"
        >
          ← Back to Enrollments
        </button>
      </div>

      <div className="form-container">
        <form onSubmit={handleSubmit} className="enrollment-form">
          {errors.general && (
            <div className="alert alert-error">
              {errors.general}
            </div>
          )}

          <div className="form-group">
            <label htmlFor="studentId" className="form-label">
              Student *
            </label>
            <select
              id="studentId"
              name="studentId"
              value={formData.studentId}
              onChange={handleChange}
              className={`form-control ${errors.studentId ? 'error' : ''}`}
              disabled={loading}
            >
              <option value={0}>Select a student</option>
              {students.map((student) => (
                <option key={student.id} value={student.id}>
                  {getStudentName(student)} (ID: {student.id})
                </option>
              ))}
            </select>
            {errors.studentId && <span className="error-message">{errors.studentId}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="courseId" className="form-label">
              Course *
            </label>
            <select
              id="courseId"
              name="courseId"
              value={formData.courseId}
              onChange={handleChange}
              className={`form-control ${errors.courseId ? 'error' : ''}`}
              disabled={loading}
            >
              <option value={0}>Select a course</option>
              {courses.map((course) => (
                <option key={course.id} value={course.id}>
                  {getCourseName(course)} (ID: {course.id})
                </option>
              ))}
            </select>
            {errors.courseId && <span className="error-message">{errors.courseId}</span>}
          </div>

          <div className="form-actions">
            <button
              type="button"
              onClick={() => navigate('/enrollments')}
              className="btn btn-secondary"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? 'Creating...' : 'Create Enrollment'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EnrollmentForm;
