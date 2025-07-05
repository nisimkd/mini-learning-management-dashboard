using AutoMapper;
using LearningManagementDashboard.Api.Constants;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Services;

/// <summary>
/// Service implementation for enrollment operations with AutoMapper integration
/// </summary>
public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
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
                var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);
                enrollmentDto.StudentName = student.FullName;
                enrollmentDto.CourseName = course.Name;
                enrollmentDtos.Add(enrollmentDto);
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

        var enrollmentDto = _mapper.Map<EnrollmentDto>(enrollment);
        enrollmentDto.StudentName = student.FullName;
        enrollmentDto.CourseName = course.Name;
        return enrollmentDto;
    }

    public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto createEnrollmentDto)
    {
        // Validate that student exists
        var student = await _studentRepository.GetByIdAsync(createEnrollmentDto.StudentId);
        if (student == null)
            throw new NotFoundException(ApiConstants.Messages.StudentNotFound);

        // Validate that course exists
        var course = await _courseRepository.GetByIdAsync(createEnrollmentDto.CourseId);
        if (course == null)
            throw new NotFoundException(ApiConstants.Messages.CourseNotFound);

        // Check if student is already enrolled
        var isAlreadyEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(
            createEnrollmentDto.StudentId, 
            createEnrollmentDto.CourseId);

        if (isAlreadyEnrolled)
            throw new ConflictException(ApiConstants.Messages.StudentAlreadyEnrolled);

        // Create enrollment using AutoMapper
        var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
        
        var createdEnrollment = await _enrollmentRepository.CreateAsync(enrollment);
        
        var enrollmentDto = _mapper.Map<EnrollmentDto>(createdEnrollment);
        enrollmentDto.StudentName = student.FullName;
        enrollmentDto.CourseName = course.Name;
        return enrollmentDto;
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id);
        if (enrollment == null)
            throw new NotFoundException(ApiConstants.Messages.EnrollmentNotFound);

        return await _enrollmentRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<EnrollmentReportDto>> GenerateEnrollmentReportAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        var students = await _studentRepository.GetAllAsync();
        
        var courseEnrollments = new List<CourseEnrollmentDto>();
        
        foreach (var course in courses)
        {
            var enrollmentCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
            courseEnrollments.Add(new CourseEnrollmentDto
            {
                CourseName = course.Name,
                EnrolledStudentsCount = enrollmentCount
            });
        }

        var report = new EnrollmentReportDto
        {
            TotalStudents = students.Count(),
            TotalCourses = courses.Count(),
            CourseEnrollments = courseEnrollments
        };

        return new List<EnrollmentReportDto> { report };
    }
}
