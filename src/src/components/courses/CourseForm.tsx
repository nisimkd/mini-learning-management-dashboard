import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { CreateCourseDto, UpdateCourseDto, FormErrors } from '../../types';
import { courseApi } from '../../services/api';
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
    name: '',
    description: ''
  });

  useEffect(() => {
    if (isEdit && id) {
      fetchCourse(parseInt(id));
    }
  }, [isEdit, id]);

  const fetchCourse = async (courseId: number) => {
    try {
      setLoading(true);
      const course = await courseApi.getById(courseId);
      if (course) {
        setFormData({
          name: course.name,
          description: course.description
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

    if (!formData.name.trim()) {
      newErrors.name = 'Name is required';
    } else if (formData.name.length < 3) {
      newErrors.name = 'Name must be at least 3 characters long';
    }

    if (!formData.description.trim()) {
      newErrors.description = 'Description is required';
    } else if (formData.description.length < 10) {
      newErrors.description = 'Description must be at least 10 characters long';
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
        await courseApi.update(parseInt(id), formData as UpdateCourseDto);
      } else {
        await courseApi.create(formData as CreateCourseDto);
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
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
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
            <label htmlFor="name" className="form-label">
              Course Name *
            </label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              className={`form-control ${errors.name ? 'error' : ''}`}
              placeholder="Enter course name"
              disabled={loading}
            />
            {errors.name && <span className="error-message">{errors.name}</span>}
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
