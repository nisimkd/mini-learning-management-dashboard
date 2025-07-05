using AutoMapper;
using LearningManagementDashboard.Api.Constants;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Services;

/// <summary>
/// Service implementation for student operations with AutoMapper integration
/// </summary>
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IMapper _mapper;

    public StudentService(
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IEnrollmentRepository enrollmentRepository,
        IMapper mapper)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _enrollmentRepository = enrollmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        var studentDtos = new List<StudentDto>();

        foreach (var student in students)
        {
            var enrollments = await _enrollmentRepository.GetByStudentIdAsync(student.Id);
            var enrolledCourses = new List<CourseDto>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    var courseEnrollmentCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
                    var courseDto = _mapper.Map<CourseDto>(course);
                    courseDto.EnrolledStudents = courseEnrollmentCount;
                    enrolledCourses.Add(courseDto);
                }
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            studentDto.EnrolledCourses = enrolledCourses;
            studentDtos.Add(studentDto);
        }

        return studentDtos;
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            return null;

        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(student.Id);
        var enrolledCourses = new List<CourseDto>();

        foreach (var enrollment in enrollments)
        {
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            if (course != null)
            {
                var courseEnrollmentCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
                var courseDto = _mapper.Map<CourseDto>(course);
                courseDto.EnrolledStudents = courseEnrollmentCount;
                enrolledCourses.Add(courseDto);
            }
        }

        var studentDto = _mapper.Map<StudentDto>(student);
        studentDto.EnrolledCourses = enrolledCourses;
        return studentDto;
    }

    public async Task<IEnumerable<StudentDto>> SearchStudentsAsync(string searchTerm)
    {
        var students = await _studentRepository.GetAllAsync();
        var filteredStudents = students.Where(s => 
            s.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            s.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        var studentDtos = new List<StudentDto>();

        foreach (var student in filteredStudents)
        {
            var enrollments = await _enrollmentRepository.GetByStudentIdAsync(student.Id);
            var enrolledCourses = new List<CourseDto>();

            foreach (var enrollment in enrollments)
            {
                var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
                if (course != null)
                {
                    var courseEnrollmentCount = await _enrollmentRepository.GetEnrollmentCountByCourseAsync(course.Id);
                    var courseDto = _mapper.Map<CourseDto>(course);
                    courseDto.EnrolledStudents = courseEnrollmentCount;
                    enrolledCourses.Add(courseDto);
                }
            }

            var studentDto = _mapper.Map<StudentDto>(student);
            studentDto.EnrolledCourses = enrolledCourses;
            studentDtos.Add(studentDto);
        }

        return studentDtos;
    }

    public async Task<EnrollmentReportDto> GetEnrollmentReportAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        var courses = await _courseRepository.GetAllAsync();
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

        return new EnrollmentReportDto
        {
            TotalStudents = students.Count(),
            TotalCourses = courses.Count(),
            CourseEnrollments = courseEnrollments
        };
    }
}
