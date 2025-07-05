using Microsoft.AspNetCore.Mvc;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;

namespace LearningManagementDashboard.Api.Controllers;

/// <summary>
/// Controller for managing student operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentRepository studentRepository, ILogger<StudentsController> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all students
    /// </summary>
    /// <returns>List of students</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Student>>>> GetAllStudents()
    {
        try
        {
            var students = await _studentRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Student>>
            {
                Success = true,
                Data = students,
                Message = "Students retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving students");
            return StatusCode(500, new ApiResponse<IEnumerable<Student>>
            {
                Success = false,
                Message = "An error occurred while retrieving students"
            });
        }
    }

    /// <summary>
    /// Get a specific student by ID
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Student>>> GetStudentById(int id)
    {
        try
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound(new ApiResponse<Student>
                {
                    Success = false,
                    Message = $"Student with ID {id} not found"
                });
            }

            return Ok(new ApiResponse<Student>
            {
                Success = true,
                Data = student,
                Message = "Student retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving student with ID {StudentId}", id);
            return StatusCode(500, new ApiResponse<Student>
            {
                Success = false,
                Message = "An error occurred while retrieving the student"
            });
        }
    }
}
