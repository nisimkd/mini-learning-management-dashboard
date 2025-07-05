namespace LearningManagementDashboard.Api.DTOs;

/// <summary>
/// Data transfer object for student response with enrolled courses
/// </summary>
public class StudentDto
{
    /// <summary>
    /// Student identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// List of courses the student is enrolled in
    /// </summary>
    public List<CourseDto> EnrolledCourses { get; set; } = new();
}

/// <summary>
/// Data transfer object for creating a new student
/// </summary>
public class CreateStudentDto
{
    /// <summary>
    /// Student's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for updating an existing student
/// </summary>
public class UpdateStudentDto
{
    /// <summary>
    /// Student's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
