using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.DTOs;

/// <summary>
/// Data transfer object for creating a new enrollment
/// </summary>
public class CreateEnrollmentDto
{
    /// <summary>
    /// ID of the student to enroll
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// ID of the course to enroll in
    /// </summary>
    public int CourseId { get; set; }
}

/// <summary>
/// Data transfer object for enrollment response
/// </summary>
public class EnrollmentDto
{
    /// <summary>
    /// Enrollment identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student identifier
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course identifier
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Student's full name
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Student's email
    /// </summary>
    public string StudentEmail { get; set; } = string.Empty;

    /// <summary>
    /// Course name
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Current enrollment status
    /// </summary>
    public EnrollmentStatus Status { get; set; }
}

/// <summary>
/// Data transfer object for enrollment report
/// </summary>
public class EnrollmentReportDto
{
    /// <summary>
    /// Total number of students
    /// </summary>
    public int TotalStudents { get; set; }

    /// <summary>
    /// Total number of courses
    /// </summary>
    public int TotalCourses { get; set; }

    /// <summary>
    /// Course enrollment details
    /// </summary>
    public List<CourseEnrollmentDto> CourseEnrollments { get; set; } = new();
}

/// <summary>
/// Data transfer object for course enrollment details
/// </summary>
public class CourseEnrollmentDto
{
    /// <summary>
    /// Course name
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Number of enrolled students
    /// </summary>
    public int EnrolledStudentsCount { get; set; }
}
