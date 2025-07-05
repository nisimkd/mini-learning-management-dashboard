using LearningManagementDashboard.Api.DTOs;

namespace LearningManagementDashboard.Api.Services.Interfaces;

/// <summary>
/// Interface for course service operations
/// </summary>
public interface ICourseService
{
    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>Collection of course DTOs</returns>
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();

    /// <summary>
    /// Get a course by its identifier
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <returns>Course DTO if found, null otherwise</returns>
    Task<CourseDto?> GetCourseByIdAsync(int id);

    /// <summary>
    /// Create a new course
    /// </summary>
    /// <param name="createCourseDto">Course creation data</param>
    /// <returns>Created course DTO</returns>
    Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);

    /// <summary>
    /// Update an existing course
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <param name="updateCourseDto">Course update data</param>
    /// <returns>Updated course DTO if found, null otherwise</returns>
    Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateCourseDto);

    /// <summary>
    /// Delete a course by its identifier
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteCourseAsync(int id);
}
