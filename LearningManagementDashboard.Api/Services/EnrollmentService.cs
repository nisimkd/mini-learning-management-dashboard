using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Services;

/// <summary>
/// Service implementation for enrollment operations
/// </summary>
public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllAsync();
        var enrollmentDtos = new List<EnrollmentDto>();

        foreach (var enrollment in enrollments)
        {
            var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);

            if (student != null && course != null)
            {
                enrollmentDtos.Add(MapToEnrollmentDto(enrollment, student, course));
            }
        }

        return enrollmentDtos;
    }

    public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
            return null;

        var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
        var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);

        if (student == null || course == null)
            return null;

        return MapToEnrollmentDto(enrollment, student, course);
    }

    public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto)
    {
        // Validate that student exists
        var student = await _studentRepository.GetByIdAsync(createEnrollmentDto.StudentId);
        if (student == null)
            throw new NotFoundException($"Student with ID {createEnrollmentDto.StudentId} not found.");

        // Validate that course exists
        var course = await _courseRepository.GetByIdAsync(createEnrollmentDto.CourseId);
        if (course == null)
            throw new NotFoundException($"Course with ID {createEnrollmentDto.CourseId} not found.");

        // Check if course is active
        if (!course.IsActive)
            throw new BusinessRuleViolationException("Cannot enroll in an inactive course.");

        // Check if student is already enrolled
        var isAlreadyEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(
            createEnrollmentDto.StudentId, 
            createEnrollmentDto.CourseId);

        if (isAlreadyEnrolled)
            throw new BusinessRuleViolationException("Student is already enrolled in this course.");

        // Check if course has capacity
        var currentEnrollments = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(createEnrollmentDto.CourseId);
        if (currentEnrollments >= course.MaxCapacity)
            throw new BusinessRuleViolationException("Course has reached maximum capacity.");

        // Create enrollment
        var enrollment = new Enrollment
        {
            StudentId = createEnrollmentDto.StudentId,
            CourseId = createEnrollmentDto.CourseId,
            Status = EnrollmentStatus.Active
        };

        var createdEnrollment = await _enrollmentRepository.CreateAsync(enrollment);
        return MapToEnrollmentDto(createdEnrollment, student, course);
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
            return false;

        return await _enrollmentRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<EnrollmentReportDto>> GenerateEnrollmentReportAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        var reportDtos = new List<EnrollmentReportDto>();

        foreach (var course in courses)
        {
            var enrollments = await _enrollmentRepository.GetByCourseIdAsync(course.Id);
            var enrollmentList = enrollments.ToList();

            var totalEnrollments = enrollmentList.Count;
            var activeEnrollments = enrollmentList.Count(e => e.Status == EnrollmentStatus.Active);
            var completedEnrollments = enrollmentList.Count(e => e.Status == EnrollmentStatus.Completed);
            var droppedEnrollments = enrollmentList.Count(e => e.Status == EnrollmentStatus.Dropped);

            var capacityUtilization = course.MaxCapacity > 0 
                ? (decimal)activeEnrollments / course.MaxCapacity * 100 
                : 0;

            reportDtos.Add(new EnrollmentReportDto
            {
                CourseId = course.Id,
                CourseTitle = course.Title,
                CourseCode = course.Code,
                TotalEnrollments = totalEnrollments,
                ActiveEnrollments = activeEnrollments,
                CompletedEnrollments = completedEnrollments,
                DroppedEnrollments = droppedEnrollments,
                CapacityUtilization = Math.Round(capacityUtilization, 2)
            });
        }

        return reportDtos.OrderBy(r => r.CourseCode);
    }

    private static EnrollmentDto MapToEnrollmentDto(Enrollment enrollment, Student student, Course course)
    {
        return new EnrollmentDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            StudentName = student.FullName,
            StudentEmail = student.Email,
            CourseTitle = course.Title,
            CourseCode = course.Code,
            EnrollmentDate = enrollment.EnrollmentDate,
            Status = enrollment.Status
        };
    }
}
