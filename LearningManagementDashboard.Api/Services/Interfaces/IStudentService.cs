using LearningManagementDashboard.Api.DTOs;

namespace LearningManagementDashboard.Api.Services.Interfaces;

/// <summary>
/// Service interface for student operations
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Get all students with their enrolled courses
    /// </summary>
    /// <returns>List of students with enrolled courses</returns>
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();

    /// <summary>
    /// Get student by ID with enrolled courses
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student with enrolled courses or null if not found</returns>
    Task<StudentDto?> GetStudentByIdAsync(int id);

    /// <summary>
    /// Search students by name or email
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching students</returns>
    Task<IEnumerable<StudentDto>> SearchStudentsAsync(string searchTerm);

    /// <summary>
    /// Get enrollment report
    /// </summary>
    /// <returns>Enrollment report with statistics</returns>
    Task<EnrollmentReportDto> GetEnrollmentReportAsync();
}
