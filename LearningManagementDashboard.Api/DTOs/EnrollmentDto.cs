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
    /// Course title
    /// </summary>
    public string CourseTitle { get; set; } = string.Empty;

    /// <summary>
    /// Course code
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

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
    /// Course identifier
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Course title
    /// </summary>
    public string CourseTitle { get; set; } = string.Empty;

    /// <summary>
    /// Course code
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Total number of enrolled students
    /// </summary>
    public int TotalEnrollments { get; set; }

    /// <summary>
    /// Number of active enrollments
    /// </summary>
    public int ActiveEnrollments { get; set; }

    /// <summary>
    /// Number of completed enrollments
    /// </summary>
    public int CompletedEnrollments { get; set; }

    /// <summary>
    /// Number of dropped enrollments
    /// </summary>
    public int DroppedEnrollments { get; set; }

    /// <summary>
    /// Course capacity utilization percentage
    /// </summary>
    public decimal CapacityUtilization { get; set; }
}
