import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { EnrollmentDto, CourseDto, Student, FormErrors } from '../../types';
import api from '../../services/api';
import './EnrollmentForm.css';

const EnrollmentForm: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [courses, setCourses] = useState<CourseDto[]>([]);
  const [students, setStudents] = useState<Student[]>([]);
  const [errors, setErrors] = useState<FormErrors>({});
  const [formData, setFormData] = useState<EnrollmentDto>({
    studentId: 0,
    courseId: 0,
    status: 'Active',
    grade: undefined
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const [coursesData, studentsData] = await Promise.all([
        api.courses.getAll(),
        api.students.getAll(),
      ]);
      
      setCourses(coursesData.filter(c => c.isActive));
      setStudents(studentsData.filter(s => s.isActive));
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

    if (!formData.status) {
      newErrors.status = 'Please select a status';
    }

    if (formData.grade !== undefined && (formData.grade < 0 || formData.grade > 100)) {
      newErrors.grade = 'Grade must be between 0 and 100';
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

      await api.enrollments.create(formData);
      navigate('/enrollments');
    } catch (error) {
      console.error('Error creating enrollment:', error);
      setErrors({ 
        general: 'Failed to create enrollment. Please try again.' 
      });
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'studentId' || name === 'courseId' ? parseInt(value) || 0 :
               name === 'grade' ? (value === '' ? undefined : parseFloat(value)) :
               value
    }));
  };

  const getStudentName = (student: Student) => {
    return `${student.firstName} ${student.lastName}`;
  };

  if (loading && courses.length === 0) {
    return (
      <div className="enrollment-form-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading data...</p>
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
              {students.map(student => (
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
              {courses.map(course => (
                <option key={course.id} value={course.id}>
                  {course.title} ({course.code}) - {course.enrolledStudents}/{course.maxCapacity}
                </option>
              ))}
            </select>
            {errors.courseId && <span className="error-message">{errors.courseId}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="status" className="form-label">
              Status *
            </label>
            <select
              id="status"
              name="status"
              value={formData.status}
              onChange={handleChange}
              className={`form-control ${errors.status ? 'error' : ''}`}
              disabled={loading}
            >
              <option value="Active">Active</option>
              <option value="Completed">Completed</option>
              <option value="Dropped">Dropped</option>
            </select>
            {errors.status && <span className="error-message">{errors.status}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="grade" className="form-label">
              Grade (optional)
            </label>
            <input
              type="number"
              id="grade"
              name="grade"
              value={formData.grade || ''}
              onChange={handleChange}
              className={`form-control ${errors.grade ? 'error' : ''}`}
              min="0"
              max="100"
              step="0.1"
              placeholder="Enter grade (0-100)"
              disabled={loading}
            />
            {errors.grade && <span className="error-message">{errors.grade}</span>}
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
