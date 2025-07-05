using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Repositories.Interfaces;

/// <summary>
/// Interface for student repository operations
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Get all students
    /// </summary>
    /// <returns>Collection of all students</returns>
    Task<IEnumerable<Student>> GetAllAsync();

    /// <summary>
    /// Get a student by their identifier
    /// </summary>
    /// <param name="id">Student identifier</param>
    /// <returns>Student if found, null otherwise</returns>
    Task<Student?> GetByIdAsync(int id);

    /// <summary>
    /// Create a new student
    /// </summary>
    /// <param name="student">Student to create</param>
    /// <returns>Created student with assigned identifier</returns>
    Task<Student> CreateAsync(Student student);

    /// <summary>
    /// Update an existing student
    /// </summary>
    /// <param name="student">Student to update</param>
    /// <returns>Updated student</returns>
    Task<Student> UpdateAsync(Student student);

    /// <summary>
    /// Delete a student by their identifier
    /// </summary>
    /// <param name="id">Student identifier</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Check if a student exists
    /// </summary>
    /// <param name="id">Student identifier</param>
    /// <returns>True if student exists, false otherwise</returns>
    Task<bool> ExistsAsync(int id);
}
