import { useState, useCallback } from 'react';
import { enrollmentApi } from '../services/api';
import { Enrollment, CreateEnrollmentDto } from '../types';
import { useFetch, useAsyncOperation } from './useAsync';

export function useEnrollments() {
  const {
    data: enrollments,
    isLoading,
    error,
    refetch,
  } = useFetch<Enrollment[]>(enrollmentApi.getAll, []);

  return {
    enrollments: enrollments || [],
    isLoading,
    error,
    refetch,
  };
}

export function useEnrollment(id?: number) {
  const {
    data: enrollment,
    isLoading,
    error,
    refetch,
  } = useFetch<Enrollment | null>(
    () => id ? enrollmentApi.getById(id) : Promise.resolve(null),
    [id]
  );

  return {
    enrollment,
    isLoading,
    error,
    refetch,
  };
}

export function useEnrollmentOperations() {
  const { execute: createEnrollment, isLoading: isCreating } = useAsyncOperation<Enrollment>();
  const { execute: deleteEnrollment, isLoading: isDeleting } = useAsyncOperation<void>();

  const handleCreateEnrollment = useCallback(async (enrollmentData: CreateEnrollmentDto) => {
    return await createEnrollment(
      () => enrollmentApi.create(enrollmentData),
      {
        showSuccessToast: true,
        successMessage: 'Student enrolled successfully!',
      }
    );
  }, [createEnrollment]);

  const handleDeleteEnrollment = useCallback(async (id: number) => {
    return await deleteEnrollment(
      () => enrollmentApi.delete(id),
      {
        showSuccessToast: true,
        successMessage: 'Enrollment removed successfully!',
      }
    );
  }, [deleteEnrollment]);

  return {
    createEnrollment: handleCreateEnrollment,
    deleteEnrollment: handleDeleteEnrollment,
    isCreating,
    isDeleting,
  };
}
