using LearningManagementDashboard.Api.DTOs;

namespace LearningManagementDashboard.Api.Services.Interfaces;

/// <summary>
/// Interface for enrollment service operations
/// </summary>
public interface IEnrollmentService
{
    /// <summary>
    /// Get all enrollments
    /// </summary>
    /// <returns>Collection of enrollment DTOs</returns>
    Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync();

    /// <summary>
    /// Get an enrollment by its identifier
    /// </summary>
    /// <param name="id">Enrollment identifier</param>
    /// <returns>Enrollment DTO if found, null otherwise</returns>
    Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);

    /// <summary>
    /// Create a new enrollment
    /// </summary>
    /// <param name="createEnrollmentDto">Enrollment creation data</param>
    /// <returns>Created enrollment DTO</returns>
    Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto);

    /// <summary>
    /// Delete an enrollment by its identifier
    /// </summary>
    /// <param name="id">Enrollment identifier</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteEnrollmentAsync(int id);

    /// <summary>
    /// Generate enrollment report
    /// </summary>
    /// <returns>Collection of enrollment report DTOs</returns>
    Task<IEnumerable<EnrollmentReportDto>> GenerateEnrollmentReportAsync();
}
