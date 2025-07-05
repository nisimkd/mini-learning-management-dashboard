using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Repositories.Interfaces;

/// <summary>
/// Interface for course repository operations
/// </summary>
public interface ICourseRepository
{
    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>Collection of all courses</returns>
    Task<IEnumerable<Course>> GetAllAsync();

    /// <summary>
    /// Get a course by its identifier
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <returns>Course if found, null otherwise</returns>
    Task<Course?> GetByIdAsync(int id);

    /// <summary>
    /// Create a new course
    /// </summary>
    /// <param name="course">Course to create</param>
    /// <returns>Created course with assigned identifier</returns>
    Task<Course> CreateAsync(Course course);

    /// <summary>
    /// Update an existing course
    /// </summary>
    /// <param name="course">Course to update</param>
    /// <returns>Updated course</returns>
    Task<Course> UpdateAsync(Course course);

    /// <summary>
    /// Delete a course by its identifier
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Check if a course exists
    /// </summary>
    /// <param name="id">Course identifier</param>
    /// <returns>True if course exists, false otherwise</returns>
    Task<bool> ExistsAsync(int id);
}
