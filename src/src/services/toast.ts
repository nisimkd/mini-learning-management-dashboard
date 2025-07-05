import { toast, ToastOptions, Id } from 'react-toastify';

// Default toast options
const defaultOptions: ToastOptions = {
  position: 'top-right',
  autoClose: 5000,
  hideProgressBar: false,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  progress: undefined,
};

export const toastService = {
  success: (message: string, options?: ToastOptions) => {
    toast.success(message, { ...defaultOptions, ...options });
  },

  error: (message: string, options?: ToastOptions) => {
    toast.error(message, { ...defaultOptions, ...options });
  },

  warning: (message: string, options?: ToastOptions) => {
    toast.warning(message, { ...defaultOptions, ...options });
  },

  info: (message: string, options?: ToastOptions) => {
    toast.info(message, { ...defaultOptions, ...options });
  },

  loading: (message: string) => {
    return toast.loading(message, defaultOptions);
  },

  updateLoading: (toastId: Id, message: string, type: 'success' | 'error' | 'warning' | 'info') => {
    toast.update(toastId, {
      render: message,
      type,
      isLoading: false,
      autoClose: 5000,
    });
  },

  promise: async <T>(
    promise: Promise<T>,
    messages: {
      loading: string;
      success: string;
      error: string;
    }
  ) => {
    return toast.promise(promise, messages, defaultOptions);
  },

  dismiss: (toastId?: Id) => {
    toast.dismiss(toastId);
  },

  dismissAll: () => {
    toast.dismiss();
  },
};

// Helper function to handle API errors
export const handleApiError = (error: any) => {
  if (error.response?.data?.message) {
    toastService.error(error.response.data.message);
  } else if (error.message) {
    toastService.error(error.message);
  } else {
    toastService.error('An unexpected error occurred');
  }
};
