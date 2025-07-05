// API Types matching the backend DTOs and models

export interface Course {
  id: number;
  title: string;
  description: string;
  code: string;
  maxCapacity: number;
  enrolledStudents: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface Student {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  enrollmentDate: Date;
  isActive: boolean;
}

export interface Enrollment {
  id: number;
  studentId: number;
  courseId: number;
  enrollmentDate: Date;
  status: string;
  grade?: number;
  // Navigation properties
  student?: Student;
  course?: Course;
}

export interface CreateCourseDto {
  title: string;
  description: string;
  code: string;
  maxCapacity: number;
}

export interface UpdateCourseDto {
  title: string;
  description: string;
  code: string;
  maxCapacity: number;
  isActive: boolean;
}

export interface CourseDto {
  id: number;
  title: string;
  description: string;
  code: string;
  maxCapacity: number;
  enrolledStudents: number;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface EnrollmentDto {
  studentId: number;
  courseId: number;
  status: string;
  grade?: number;
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}

export interface EnrollmentReport {
  totalEnrollments: number;
  courseEnrollments: Array<{
    courseId: number;
    courseName: string;
    enrollmentCount: number;
    capacity: number;
    utilizationPercentage: number;
  }>;
  studentEnrollments: Array<{
    studentId: number;
    studentName: string;
    courseCount: number;
    courses: string[];
  }>;
}

// UI-specific types
export interface LoadingState {
  isLoading: boolean;
  error: string | null;
}

export interface FormErrors {
  [key: string]: string;
}
