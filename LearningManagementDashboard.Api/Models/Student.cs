namespace LearningManagementDashboard.Api.Models;

/// <summary>
/// Represents a student in the learning management system
/// </summary>
public class Student
{
    /// <summary>
    /// Unique identifier for the student
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Student's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Student's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Date when the student was registered
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Full name of the student
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
