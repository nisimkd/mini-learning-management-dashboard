namespace LearningManagementDashboard.Api.DTOs;

/// <summary>
/// Data transfer object for creating a new course
/// </summary>
public class CreateCourseDto
{
    /// <summary>
    /// Course name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for updating an existing course
/// </summary>
public class UpdateCourseDto
{
    /// <summary>
    /// Course name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course description
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for course response
/// </summary>
public class CourseDto
{
    /// <summary>
    /// Course identifier
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
    /// Number of currently enrolled students
    /// </summary>
    public int EnrolledStudents { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
