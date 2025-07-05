using AutoMapper;
using LearningManagementDashboard.Api.Constants;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Services;

/// <summary>
/// Service implementation for course operations with AutoMapper integration
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;

    public CourseService(
        ICourseRepository courseRepository, 
        IEnrollmentRepository enrollmentRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        var courseDtos = new List<CourseDto>();

        foreach (var course in courses)
        {
            var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
            var courseDto = _mapper.Map<CourseDto>(course);
            courseDto.EnrolledStudents = enrolledCount;
            courseDtos.Add(courseDto);
        }

        return courseDtos;
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            return null;

        var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
        var courseDto = _mapper.Map<CourseDto>(course);
        courseDto.EnrolledStudents = enrolledCount;
        return courseDto;
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        var course = _mapper.Map<Course>(createCourseDto);
        var createdCourse = await _courseRepository.CreateAsync(course);
        
        var courseDto = _mapper.Map<CourseDto>(createdCourse);
        courseDto.EnrolledStudents = 0; // New course has no enrollments
        return courseDto;
    }

    public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateCourseDto)
    {
        var existingCourse = await _courseRepository.GetByIdAsync(id);
        if (existingCourse == null)
            return null;

        // Use AutoMapper to update the existing course
        _mapper.Map(updateCourseDto, existingCourse);
        
        var updatedCourse = await _courseRepository.UpdateAsync(existingCourse);
        var enrolledCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(updatedCourse.Id);
        
        var courseDto = _mapper.Map<CourseDto>(updatedCourse);
        courseDto.EnrolledStudents = enrolledCount;
        return courseDto;
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        // Check if course exists
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException(ApiConstants.Messages.CourseNotFound);

        // Check if course has any enrollments (business rule)
        var enrollments = await _enrollmentRepository.GetByCourseIdAsync(id);
        if (enrollments.Any())
            throw new ConflictException(ApiConstants.Messages.CourseHasEnrollments);

        return await _courseRepository.DeleteAsync(id);
    }
}
