using Microsoft.AspNetCore.Mvc;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Services.Interfaces;

namespace LearningManagementDashboard.Api.Controllers;

/// <summary>
/// Controller for managing student operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all students with their enrolled courses
    /// </summary>
    /// <returns>List of students with enrolled courses</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentDto>>>> GetAllStudents()
    {
        try
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<StudentDto>>
            {
                Success = true,
                Data = students,
                Message = "Students retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving students");
            return StatusCode(500, new ApiResponse<IEnumerable<StudentDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving students"
            });
        }
    }

    /// <summary>
    /// Get student by ID with enrolled courses
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student with enrolled courses</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetStudentById(int id)
    {
        try
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(new ApiResponse<StudentDto>
                {
                    Success = false,
                    Message = "Student not found"
                });
            }

            return Ok(new ApiResponse<StudentDto>
            {
                Success = true,
                Data = student,
                Message = "Student retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving student with ID {Id}", id);
            return StatusCode(500, new ApiResponse<StudentDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the student"
            });
        }
    }

    /// <summary>
    /// Search students by name or email
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching students</returns>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentDto>>>> SearchStudents([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new ApiResponse<IEnumerable<StudentDto>>
                {
                    Success = false,
                    Message = "Search term is required"
                });
            }

            var students = await _studentService.SearchStudentsAsync(searchTerm);
            return Ok(new ApiResponse<IEnumerable<StudentDto>>
            {
                Success = true,
                Data = students,
                Message = "Students search completed successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching students with term {SearchTerm}", searchTerm);
            return StatusCode(500, new ApiResponse<IEnumerable<StudentDto>>
            {
                Success = false,
                Message = "An error occurred while searching students"
            });
        }
    }

    /// <summary>
    /// Get enrollment report
    /// </summary>
    /// <returns>Enrollment report with statistics</returns>
    [HttpGet("report")]
    public async Task<ActionResult<ApiResponse<EnrollmentReportDto>>> GetEnrollmentReport()
    {
        try
        {
            var report = await _studentService.GetEnrollmentReportAsync();
            return Ok(new ApiResponse<EnrollmentReportDto>
            {
                Success = true,
                Data = report,
                Message = "Enrollment report generated successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating enrollment report");
            return StatusCode(500, new ApiResponse<EnrollmentReportDto>
            {
                Success = false,
                Message = "An error occurred while generating the enrollment report"
            });
        }
    }
}
