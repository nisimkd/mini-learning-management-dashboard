namespace LearningManagementDashboard.Api.DTOs;

/// <summary>
/// Data transfer object for creating a new course
/// </summary>
public class CreateCourseDto
{
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
}

/// <summary>
/// Data transfer object for updating an existing course
/// </summary>
public class UpdateCourseDto
{
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
    /// Whether the course is currently active
    /// </summary>
    public bool IsActive { get; set; }
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
    /// Course title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Course description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Course code
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Maximum capacity
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Number of currently enrolled students
    /// </summary>
    public int EnrolledStudents { get; set; }

    /// <summary>
    /// Whether the course is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
