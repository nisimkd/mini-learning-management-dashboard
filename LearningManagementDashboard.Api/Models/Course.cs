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
    /// Course name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Date when the course was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
