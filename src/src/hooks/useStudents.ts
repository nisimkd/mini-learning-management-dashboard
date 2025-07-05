import { useState, useCallback } from 'react';
import { studentApi } from '../services/api';
import { Student, EnrollmentReport } from '../types';
import { useFetch, useAsyncOperation } from './useAsync';

export function useStudents() {
  const {
    data: students,
    isLoading,
    error,
    refetch,
  } = useFetch<Student[]>(studentApi.getAll, []);

  return {
    students: students || [],
    isLoading,
    error,
    refetch,
  };
}

export function useStudent(id?: number) {
  const {
    data: student,
    isLoading,
    error,
    refetch,
  } = useFetch<Student | null>(
    () => id ? studentApi.getById(id) : Promise.resolve(null),
    [id]
  );

  return {
    student,
    isLoading,
    error,
    refetch,
  };
}

export function useStudentSearch() {
  const [searchResults, setSearchResults] = useState<Student[]>([]);
  const { execute: executeSearch, isLoading: isSearching } = useAsyncOperation<Student[]>();

  const searchStudents = useCallback(async (searchTerm: string) => {
    if (!searchTerm.trim()) {
      setSearchResults([]);
      return;
    }

    const results = await executeSearch(
      () => studentApi.search(searchTerm),
      {
        showErrorToast: true,
        errorMessage: 'Failed to search students',
      }
    );

    setSearchResults(results || []);
    return results;
  }, [executeSearch]);

  const clearSearch = useCallback(() => {
    setSearchResults([]);
  }, []);

  return {
    searchResults,
    searchStudents,
    clearSearch,
    isSearching,
  };
}

export function useEnrollmentReport() {
  const {
    data: report,
    isLoading,
    error,
    refetch,
  } = useFetch<EnrollmentReport>(studentApi.getEnrollmentReport, []);

  return {
    report,
    isLoading,
    error,
    refetch,
  };
}
