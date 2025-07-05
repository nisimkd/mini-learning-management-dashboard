// API Types matching the backend DTOs and models

export interface Course {
  id: number;
  name: string;
  description: string;
  enrolledStudents: number;
  createdAt: Date;
}

export interface Student {
  id: number;
  fullName: string;
  email: string;
  enrolledCourses: Course[];
}

export interface Enrollment {
  id: number;
  studentId: number;
  courseId: number;
  enrollmentDate: Date;
  status: string;
  studentName: string;
  studentEmail: string;
  courseName: string;
}

export interface CreateCourseDto {
  name: string;
  description: string;
}

export interface UpdateCourseDto {
  name: string;
  description: string;
}

export interface CreateEnrollmentDto {
  studentId: number;
  courseId: number;
}

export interface EnrollmentReport {
  totalStudents: number;
  totalCourses: number;
  courseEnrollments: CourseEnrollment[];
}

export interface CourseEnrollment {
  courseName: string;
  enrolledStudentsCount: number;
}

// API Response wrapper
export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
}

// UI-specific types
export interface LoadingState {
  isLoading: boolean;
  error: string | null;
}

export interface FormErrors {
  [key: string]: string;
}
