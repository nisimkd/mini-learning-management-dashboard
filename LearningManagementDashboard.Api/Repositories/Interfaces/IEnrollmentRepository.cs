using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Repositories.Interfaces;

/// <summary>
/// Interface for enrollment repository operations
/// </summary>
public interface IEnrollmentRepository
{
    /// <summary>
    /// Get all enrollments
    /// </summary>
    /// <returns>Collection of all enrollments</returns>
    Task<IEnumerable<Enrollment>> GetAllAsync();

    /// <summary>
    /// Get an enrollment by its identifier
    /// </summary>
    /// <param name="id">Enrollment identifier</param>
    /// <returns>Enrollment if found, null otherwise</returns>
    Task<Enrollment?> GetByIdAsync(int id);

    /// <summary>
    /// Get all enrollments for a specific student
    /// </summary>
    /// <param name="studentId">Student identifier</param>
    /// <returns>Collection of enrollments for the student</returns>
    Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId);

    /// <summary>
    /// Get all enrollments for a specific course
    /// </summary>
    /// <param name="courseId">Course identifier</param>
    /// <returns>Collection of enrollments for the course</returns>
    Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId);

    /// <summary>
    /// Create a new enrollment
    /// </summary>
    /// <param name="enrollment">Enrollment to create</param>
    /// <returns>Created enrollment with assigned identifier</returns>
    Task<Enrollment> CreateAsync(Enrollment enrollment);

    /// <summary>
    /// Update an existing enrollment
    /// </summary>
    /// <param name="enrollment">Enrollment to update</param>
    /// <returns>Updated enrollment</returns>
    Task<Enrollment> UpdateAsync(Enrollment enrollment);

    /// <summary>
    /// Delete an enrollment by its identifier
    /// </summary>
    /// <param name="id">Enrollment identifier</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Check if a student is already enrolled in a course
    /// </summary>
    /// <param name="studentId">Student identifier</param>
    /// <param name="courseId">Course identifier</param>
    /// <returns>True if enrolled, false otherwise</returns>
    Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);

    /// <summary>
    /// Get enrollment count for a specific course
    /// </summary>
    /// <param name="courseId">Course identifier</param>
    /// <returns>Number of enrollments for the course</returns>
    Task<int> GetEnrollmentCountByCourseAsync(int courseId);
}
