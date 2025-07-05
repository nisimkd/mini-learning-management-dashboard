import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { CreateCourseDto, UpdateCourseDto, FormErrors } from '../../types';
import api from '../../services/api';
import './CourseForm.css';

interface CourseFormProps {
  isEdit?: boolean;
}

const CourseForm: React.FC<CourseFormProps> = ({ isEdit = false }) => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<FormErrors>({});
  const [formData, setFormData] = useState<CreateCourseDto | UpdateCourseDto>({
    title: '',
    description: '',
    code: '',
    maxCapacity: 20,
    ...(isEdit && { isActive: true })
  });

  useEffect(() => {
    if (isEdit && id) {
      fetchCourse(parseInt(id));
    }
  }, [isEdit, id]);

  const fetchCourse = async (courseId: number) => {
    try {
      setLoading(true);
      const course = await api.courses.getById(courseId);
      if (course) {
        setFormData({
          title: course.title,
          description: course.description,
          code: course.code,
          maxCapacity: course.maxCapacity,
          isActive: course.isActive
        });
      } else {
        setErrors({ general: 'Course not found' });
      }
    } catch (error) {
      console.error('Error fetching course:', error);
      setErrors({ general: 'Failed to load course data' });
    } finally {
      setLoading(false);
    }
  };

  const validateForm = (): boolean => {
    const newErrors: FormErrors = {};

    if (!formData.title.trim()) {
      newErrors.title = 'Title is required';
    } else if (formData.title.length < 3) {
      newErrors.title = 'Title must be at least 3 characters long';
    }

    if (!formData.description.trim()) {
      newErrors.description = 'Description is required';
    } else if (formData.description.length < 10) {
      newErrors.description = 'Description must be at least 10 characters long';
    }

    if (!formData.code.trim()) {
      newErrors.code = 'Code is required';
    } else if (formData.code.length < 2) {
      newErrors.code = 'Code must be at least 2 characters long';
    }

    if (formData.maxCapacity < 1) {
      newErrors.maxCapacity = 'Max capacity must be at least 1';
    } else if (formData.maxCapacity > 500) {
      newErrors.maxCapacity = 'Max capacity cannot exceed 500';
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

      if (isEdit && id) {
        await api.courses.update(parseInt(id), formData as UpdateCourseDto);
      } else {
        await api.courses.create(formData as CreateCourseDto);
      }

      navigate('/courses');
    } catch (error) {
      console.error('Error saving course:', error);
      setErrors({ 
        general: `Failed to ${isEdit ? 'update' : 'create'} course. Please try again.` 
      });
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'number' ? parseInt(value) || 0 : value
    }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: checked
    }));
  };

  if (loading && isEdit) {
    return (
      <div className="course-form-page">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading course...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="course-form-page">
      <div className="form-header">
        <h1>{isEdit ? 'Edit Course' : 'Create New Course'}</h1>
        <button 
          onClick={() => navigate('/courses')} 
          className="back-button"
        >
          ← Back to Courses
        </button>
      </div>

      <div className="form-container">
        <form onSubmit={handleSubmit} className="course-form">
          {errors.general && (
            <div className="alert alert-error">
              {errors.general}
            </div>
          )}

          <div className="form-group">
            <label htmlFor="title" className="form-label">
              Course Title *
            </label>
            <input
              type="text"
              id="title"
              name="title"
              value={formData.title}
              onChange={handleChange}
              className={`form-control ${errors.title ? 'error' : ''}`}
              placeholder="Enter course title"
              disabled={loading}
            />
            {errors.title && <span className="error-message">{errors.title}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="code" className="form-label">
              Course Code *
            </label>
            <input
              type="text"
              id="code"
              name="code"
              value={formData.code}
              onChange={handleChange}
              className={`form-control ${errors.code ? 'error' : ''}`}
              placeholder="e.g., CS101"
              disabled={loading}
            />
            {errors.code && <span className="error-message">{errors.code}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="description" className="form-label">
              Description *
            </label>
            <textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              className={`form-control ${errors.description ? 'error' : ''}`}
              placeholder="Enter course description"
              rows={4}
              disabled={loading}
            />
            {errors.description && <span className="error-message">{errors.description}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="maxCapacity" className="form-label">
              Maximum Capacity *
            </label>
            <input
              type="number"
              id="maxCapacity"
              name="maxCapacity"
              value={formData.maxCapacity}
              onChange={handleChange}
              className={`form-control ${errors.maxCapacity ? 'error' : ''}`}
              min="1"
              max="500"
              disabled={loading}
            />
            {errors.maxCapacity && <span className="error-message">{errors.maxCapacity}</span>}
          </div>

          {isEdit && (
            <div className="form-group">
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  name="isActive"
                  checked={(formData as UpdateCourseDto).isActive}
                  onChange={handleCheckboxChange}
                  disabled={loading}
                />
                <span className="checkbox-text">Course is active</span>
              </label>
            </div>
          )}

          <div className="form-actions">
            <button
              type="button"
              onClick={() => navigate('/courses')}
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
              {loading ? 'Saving...' : (isEdit ? 'Update Course' : 'Create Course')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CourseForm;
