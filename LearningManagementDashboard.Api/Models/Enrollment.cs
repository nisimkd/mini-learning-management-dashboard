namespace LearningManagementDashboard.Api.Models;

/// <summary>
/// Represents an enrollment relationship between a student and a course
/// </summary>
public class Enrollment
{
    /// <summary>
    /// Unique identifier for the enrollment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the enrolled student
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// ID of the course the student is enrolled in
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Date when the enrollment was created
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Current status of the enrollment
    /// </summary>
    public EnrollmentStatus Status { get; set; }

    /// <summary>
    /// Navigation property to the enrolled student
    /// </summary>
    public Student? Student { get; set; }

    /// <summary>
    /// Navigation property to the course
    /// </summary>
    public Course? Course { get; set; }
}

/// <summary>
/// Enumeration of possible enrollment statuses
/// </summary>
public enum EnrollmentStatus
{
    /// <summary>
    /// Student is actively enrolled
    /// </summary>
    Active,

    /// <summary>
    /// Student has completed the course
    /// </summary>
    Completed,

    /// <summary>
    /// Student has dropped the course
    /// </summary>
    Dropped,

    /// <summary>
    /// Enrollment is pending approval
    /// </summary>
    Pending
}
