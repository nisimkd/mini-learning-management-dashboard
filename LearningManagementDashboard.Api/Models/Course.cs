namespace LearningManagementDashboard.Api.Models;

/// <summary>
/// Represents a course in the learning management system
/// </summary>
public class Course
{
    /// <summary>
    /// Unique identifier for the course
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Course description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Course code (e.g., "CS101")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of students that can enroll in the course
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Date when the course was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the course was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Whether the course is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;
}
