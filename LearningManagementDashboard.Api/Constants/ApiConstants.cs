namespace LearningManagementDashboard.Api.Constants;

/// <summary>
/// Application-wide constants to avoid magic strings and centralize configuration values
/// </summary>
public static class ApiConstants
{
    public static class Messages
    {
        public const string Success = "Success";
        public const string StudentsRetrievedSuccessfully = "Students retrieved successfully";
        public const string StudentRetrievedSuccessfully = "Student retrieved successfully";
        public const string CourseRetrievedSuccessfully = "Course retrieved successfully";
        public const string CourseCreatedSuccessfully = "Course created successfully";
        public const string CourseUpdatedSuccessfully = "Course updated successfully";
        public const string CourseDeletedSuccessfully = "Course deleted successfully";
        public const string EnrollmentCreatedSuccessfully = "Enrollment created successfully";
        public const string EnrollmentDeletedSuccessfully = "Enrollment deleted successfully";
        public const string CourseNotFound = "Course not found";
        public const string StudentNotFound = "Student not found";
        public const string EnrollmentNotFound = "Enrollment not found";
        public const string CourseHasEnrollments = "Cannot delete course with existing enrollments";
        public const string StudentAlreadyEnrolled = "Student is already enrolled in this course";
    }

    public static class Headers
    {
        public const string XContentTypeOptions = "X-Content-Type-Options";
        public const string XFrameOptions = "X-Frame-Options";
        public const string XXssProtection = "X-XSS-Protection";
        public const string ReferrerPolicy = "Referrer-Policy";
    }

    public static class HeaderValues
    {
        public const string NoSniff = "nosniff";
        public const string Deny = "DENY";
        public const string XssBlock = "1; mode=block";
        public const string StrictOriginWhenCrossOrigin = "strict-origin-when-cross-origin";
    }

    public static class EnrollmentStatus
    {
        public const string Active = "Active";
        public const string Completed = "Completed";
        public const string Dropped = "Dropped";
    }

    public static class Validation
    {
        public const int MinCourseNameLength = 3;
        public const int MaxCourseNameLength = 100;
        public const int MinCourseDescriptionLength = 10;
        public const int MaxCourseDescriptionLength = 500;
        public const int MinStudentNameLength = 2;
        public const int MaxStudentNameLength = 100;
        public const int MinEmailLength = 5;
        public const int MaxEmailLength = 100;
    }
}
