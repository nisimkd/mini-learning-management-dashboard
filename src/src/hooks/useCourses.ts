import { useState, useCallback } from 'react';
import { courseApi } from '../services/api';
import { Course, CreateCourseDto, UpdateCourseDto } from '../types';
import { useFetch, useAsyncOperation } from './useAsync';

export function useCourses() {
  const {
    data: courses,
    isLoading,
    error,
    refetch,
  } = useFetch<Course[]>(courseApi.getAll, []);

  return {
    courses: courses || [],
    isLoading,
    error,
    refetch,
  };
}

export function useCourse(id?: number) {
  const {
    data: course,
    isLoading,
    error,
    refetch,
  } = useFetch<Course | null>(
    () => id ? courseApi.getById(id) : Promise.resolve(null),
    [id]
  );

  return {
    course,
    isLoading,
    error,
    refetch,
  };
}

export function useCourseOperations() {
  const { execute: createCourse, isLoading: isCreating } = useAsyncOperation<Course>();
  const { execute: updateCourse, isLoading: isUpdating } = useAsyncOperation<Course>();
  const { execute: deleteCourse, isLoading: isDeleting } = useAsyncOperation<void>();

  const handleCreateCourse = useCallback(async (courseData: CreateCourseDto) => {
    return await createCourse(
      () => courseApi.create(courseData),
      {
        showSuccessToast: true,
        successMessage: 'Course created successfully!',
      }
    );
  }, [createCourse]);

  const handleUpdateCourse = useCallback(async (id: number, courseData: UpdateCourseDto) => {
    return await updateCourse(
      () => courseApi.update(id, courseData),
      {
        showSuccessToast: true,
        successMessage: 'Course updated successfully!',
      }
    );
  }, [updateCourse]);

  const handleDeleteCourse = useCallback(async (id: number) => {
    return await deleteCourse(
      () => courseApi.delete(id),
      {
        showSuccessToast: true,
        successMessage: 'Course deleted successfully!',
      }
    );
  }, [deleteCourse]);

  return {
    createCourse: handleCreateCourse,
    updateCourse: handleUpdateCourse,
    deleteCourse: handleDeleteCourse,
    isCreating,
    isUpdating,
    isDeleting,
  };
}
