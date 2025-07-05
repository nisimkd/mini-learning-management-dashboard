import { useState, useEffect, useCallback } from 'react';
import { toastService, handleApiError } from '../services/toast';

// Custom hook for API requests with loading and error states
export function useAsyncOperation<T = any>() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<T | null>(null);

  const execute = useCallback(async (
    operation: () => Promise<T>,
    options?: {
      showSuccessToast?: boolean;
      successMessage?: string;
      showErrorToast?: boolean;
      errorMessage?: string;
    }
  ) => {
    setIsLoading(true);
    setError(null);

    try {
      const result = await operation();
      setData(result);
      
      if (options?.showSuccessToast && options?.successMessage) {
        toastService.success(options.successMessage);
      }
      
      return result;
    } catch (err: any) {
      const errorMessage = options?.errorMessage || err?.response?.data?.message || err?.message || 'An unexpected error occurred';
      setError(errorMessage);
      
      if (options?.showErrorToast !== false) {
        toastService.error(errorMessage);
      }
      
      throw err;
    } finally {
      setIsLoading(false);
    }
  }, []);

  const reset = useCallback(() => {
    setIsLoading(false);
    setError(null);
    setData(null);
  }, []);

  return {
    isLoading,
    error,
    data,
    execute,
    reset,
  };
}

// Custom hook for fetching data with automatic loading states
export function useFetch<T = any>(
  fetchFunction: () => Promise<T>,
  dependencies: React.DependencyList = []
) {
  const [data, setData] = useState<T | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const refetch = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    try {
      const result = await fetchFunction();
      setData(result);
    } catch (err: any) {
      const errorMessage = err?.response?.data?.message || err?.message || 'Failed to fetch data';
      setError(errorMessage);
      handleApiError(err);
    } finally {
      setIsLoading(false);
    }
  }, dependencies);

  useEffect(() => {
    refetch();
  }, [refetch]);

  return {
    data,
    isLoading,
    error,
    refetch,
  };
}

// Custom hook for form submissions
export function useFormSubmission<T = any>() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const submit = useCallback(async (
    submitFunction: () => Promise<T>,
    options?: {
      successMessage?: string;
      errorMessage?: string;
      onSuccess?: (data: T) => void;
      onError?: (error: any) => void;
    }
  ) => {
    setIsSubmitting(true);
    setError(null);

    try {
      const result = await submitFunction();
      
      if (options?.successMessage) {
        toastService.success(options.successMessage);
      }
      
      if (options?.onSuccess) {
        options.onSuccess(result);
      }
      
      return result;
    } catch (err: any) {
      const errorMessage = options?.errorMessage || err?.response?.data?.message || err?.message || 'Submission failed';
      setError(errorMessage);
      
      if (options?.onError) {
        options.onError(err);
      } else {
        toastService.error(errorMessage);
      }
      
      throw err;
    } finally {
      setIsSubmitting(false);
    }
  }, []);

  return {
    isSubmitting,
    error,
    submit,
  };
}

// Custom hook for managing loading states
export function useLoading(initialState = false) {
  const [isLoading, setIsLoading] = useState(initialState);

  const withLoading = useCallback(async <T>(operation: () => Promise<T>): Promise<T> => {
    setIsLoading(true);
    try {
      return await operation();
    } finally {
      setIsLoading(false);
    }
  }, []);

  return {
    isLoading,
    setIsLoading,
    withLoading,
  };
}
