import axios, { AxiosResponse } from 'axios';
import { 
  Enrollment, 
  Student, 
  Course,
  CreateCourseDto,
  UpdateCourseDto,
  CreateEnrollmentDto,
  ApiResponse, 
  EnrollmentReport 
} from '../types';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5092/api';

// Create axios instance with default config
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to log requests in development
apiClient.interceptors.request.use(
  (config) => {
    if (process.env.NODE_ENV === 'development') {
      console.log('API Request:', config.method?.toUpperCase(), config.url);
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor to handle API responses
apiClient.interceptors.response.use(
  (response: AxiosResponse) => {
    if (process.env.NODE_ENV === 'development') {
      console.log('API Response:', response.status, response.config.url);
    }
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

// Course API methods
export const courseApi = {
  async getAll(): Promise<Course[]> {
    const response = await apiClient.get<ApiResponse<Course[]>>('/courses');
    return response.data.data || [];
  },

  async getById(id: number): Promise<Course | null> {
    try {
      const response = await apiClient.get<ApiResponse<Course>>(`/courses/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async create(courseData: CreateCourseDto): Promise<Course> {
    const response = await apiClient.post<ApiResponse<Course>>('/courses', courseData);
    return response.data.data!;
  },

  async update(id: number, courseData: UpdateCourseDto): Promise<Course> {
    const response = await apiClient.put<ApiResponse<Course>>(`/courses/${id}`, courseData);
    return response.data.data!;
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`/courses/${id}`);
  },
};

// Student API methods
export const studentApi = {
  async getAll(): Promise<Student[]> {
    const response = await apiClient.get<ApiResponse<Student[]>>('/students');
    return response.data.data || [];
  },

  async getById(id: number): Promise<Student | null> {
    try {
      const response = await apiClient.get<ApiResponse<Student>>(`/students/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async search(searchTerm: string): Promise<Student[]> {
    const response = await apiClient.get<ApiResponse<Student[]>>(`/students/search?searchTerm=${encodeURIComponent(searchTerm)}`);
    return response.data.data || [];
  },

  async getEnrollmentReport(): Promise<EnrollmentReport> {
    const response = await apiClient.get<ApiResponse<EnrollmentReport>>('/students/report');
    return response.data.data!;
  },
};

// Enrollment API methods
export const enrollmentApi = {
  async getAll(): Promise<Enrollment[]> {
    const response = await apiClient.get<ApiResponse<Enrollment[]>>('/enrollments');
    return response.data.data || [];
  },

  async getById(id: number): Promise<Enrollment | null> {
    try {
      const response = await apiClient.get<ApiResponse<Enrollment>>(`/enrollments/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async create(enrollmentData: CreateEnrollmentDto): Promise<Enrollment> {
    const response = await apiClient.post<ApiResponse<Enrollment>>('/enrollments', enrollmentData);
    return response.data.data!;
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`/enrollments/${id}`);
  },
};
