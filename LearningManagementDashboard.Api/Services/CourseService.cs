using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Services;

/// <summary>
/// Service implementation for course operations
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public CourseService(ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        var courseDtos = new List<CourseDto>();

        foreach (var course in courses)
        {
            var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
            courseDtos.Add(MapToCourseDto(course, enrolledCount));
        }

        return courseDtos;
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            return null;

        var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
        return MapToCourseDto(course, enrolledCount);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        // Validate input
        ValidateCreateCourseDto(createCourseDto);

        var course = new Course
        {
            Title = createCourseDto.Title.Trim(),
            Description = createCourseDto.Description.Trim(),
            Code = createCourseDto.Code.Trim().ToUpper(),
            MaxCapacity = createCourseDto.MaxCapacity,
            IsActive = true
        };

        var createdCourse = await _courseRepository.CreateAsync(course);
        return MapToCourseDto(createdCourse, 0);
    }

    public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateCourseDto)
    {
        var existingCourse = await _courseRepository.GetByIdAsync(id);
        if (existingCourse == null)
            return null;

        // Validate input
        ValidateUpdateCourseDto(updateCourseDto);

        // Check if reducing capacity would exceed enrolled students
        if (!updateCourseDto.IsActive || updateCourseDto.MaxCapacity < existingCourse.MaxCapacity)
        {
            var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(id);
            if (updateCourseDto.MaxCapacity < enrolledCount)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot reduce capacity to {updateCourseDto.MaxCapacity}. " +
                    $"There are currently {enrolledCount} students enrolled.");
            }
        }

        // Update course properties
        existingCourse.Title = updateCourseDto.Title.Trim();
        existingCourse.Description = updateCourseDto.Description.Trim();
        existingCourse.Code = updateCourseDto.Code.Trim().ToUpper();
        existingCourse.MaxCapacity = updateCourseDto.MaxCapacity;
        existingCourse.IsActive = updateCourseDto.IsActive;

        var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
        var enrolledCountFinal = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(updatedCourse.Id);
        
        return MapToCourseDto(updatedCourse, enrolledCountFinal);
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            return false;

        // Check if course has active enrollments
        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(id);
        var hasActiveEnrollments = enrollments.Any(e => e.Status == EnrollmentStatus.Active);

        if (hasActiveEnrollments)
        {
            throw new BusinessRuleViolationException(
                "Cannot delete a course with active enrollments. " +
                "Please remove all enrollments first or mark the course as inactive.");
        }

        return await _courseRepository.DeleteAsync(id);
    }

    private static CourseDto MapToCourseDto(Course course, int enrolledStudents)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Code = course.Code,
            MaxCapacity = course.MaxCapacity,
            EnrolledStudents = enrolledStudents,
            IsActive = course.IsActive,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }

    private static void ValidateCreateCourseDto(CreateCourseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ValidationException("Course title is required.");

        if (string.IsNullOrWhiteSpace(dto.Code))
            throw new ValidationException("Course code is required.");

        if (dto.MaxCapacity <= 0)
            throw new ValidationException("Course capacity must be greater than 0.");

        if (dto.MaxCapacity > 1000)
            throw new ValidationException("Course capacity cannot exceed 1000 students.");
    }

    private static void ValidateUpdateCourseDto(UpdateCourseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ValidationException("Course title is required.");

        if (string.IsNullOrWhiteSpace(dto.Code))
            throw new ValidationException("Course code is required.");

        if (dto.MaxCapacity <= 0)
            throw new ValidationException("Course capacity must be greater than 0.");

        if (dto.MaxCapacity > 1000)
            throw new ValidationException("Course capacity cannot exceed 1000 students.");
    }
}
