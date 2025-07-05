import axios, { AxiosResponse } from 'axios';
import { 
  Enrollment, 
  Student, 
  CreateCourseDto,
  UpdateCourseDto,
  CourseDto,
  EnrollmentDto, 
  ApiResponse, 
  EnrollmentReport 
} from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5091';

// Create axios instance with default config
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Response interceptor to handle API responses
apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    console.error('API Error:', error);
    return Promise.reject(error);
  }
);

// Course API methods
export const courseApi = {
  async getAll(): Promise<CourseDto[]> {
    const response = await apiClient.get<ApiResponse<CourseDto[]>>('/api/courses');
    return response.data.data || [];
  },

  async getById(id: number): Promise<CourseDto | null> {
    try {
      const response = await apiClient.get<ApiResponse<CourseDto>>(`/api/courses/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async create(course: CreateCourseDto): Promise<CourseDto> {
    const response = await apiClient.post<ApiResponse<CourseDto>>('/api/courses', course);
    if (!response.data.success || !response.data.data) {
      throw new Error(response.data.message || 'Failed to create course');
    }
    return response.data.data;
  },

  async update(id: number, course: UpdateCourseDto): Promise<CourseDto> {
    const response = await apiClient.put<ApiResponse<CourseDto>>(`/api/courses/${id}`, course);
    if (!response.data.success || !response.data.data) {
      throw new Error(response.data.message || 'Failed to update course');
    }
    return response.data.data;
  },

  async delete(id: number): Promise<void> {
    const response = await apiClient.delete<ApiResponse<void>>(`/api/courses/${id}`);
    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to delete course');
    }
  },
};

// Student API methods
export const studentApi = {
  async getAll(): Promise<Student[]> {
    const response = await apiClient.get<ApiResponse<Student[]>>('/api/students');
    return response.data.data || [];
  },

  async getById(id: number): Promise<Student | null> {
    try {
      const response = await apiClient.get<ApiResponse<Student>>(`/api/students/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },
};

// Enrollment API methods
export const enrollmentApi = {
  async getAll(): Promise<Enrollment[]> {
    const response = await apiClient.get<ApiResponse<Enrollment[]>>('/api/enrollments');
    return response.data.data || [];
  },

  async getById(id: number): Promise<Enrollment | null> {
    try {
      const response = await apiClient.get<ApiResponse<Enrollment>>(`/api/enrollments/${id}`);
      return response.data.data || null;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null;
      }
      throw error;
    }
  },

  async create(enrollment: EnrollmentDto): Promise<Enrollment> {
    const response = await apiClient.post<ApiResponse<Enrollment>>('/api/enrollments', enrollment);
    if (!response.data.success || !response.data.data) {
      throw new Error(response.data.message || 'Failed to create enrollment');
    }
    return response.data.data;
  },

  async delete(id: number): Promise<void> {
    const response = await apiClient.delete<ApiResponse<void>>(`/api/enrollments/${id}`);
    if (!response.data.success) {
      throw new Error(response.data.message || 'Failed to delete enrollment');
    }
  },

  async getReport(): Promise<EnrollmentReport> {
    const response = await apiClient.get<ApiResponse<EnrollmentReport>>('/api/enrollments/report');
    if (!response.data.success || !response.data.data) {
      throw new Error(response.data.message || 'Failed to get enrollment report');
    }
    return response.data.data;
  },
};

// Combined API object
export const api = {
  courses: courseApi,
  students: studentApi,
  enrollments: enrollmentApi,
};

export default api;
