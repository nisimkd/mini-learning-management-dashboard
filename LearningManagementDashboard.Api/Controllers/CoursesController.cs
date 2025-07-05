using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementDashboard.Api.Controllers;

/// <summary>
/// API controller for course operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>List of courses</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetAllCourses()
    {
        try
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(courses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all courses");
            return StatusCode(500, ApiResponse<IEnumerable<CourseDto>>.ErrorResponse("An error occurred while retrieving courses"));
        }
    }

    /// <summary>
    /// Get a course by ID
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourseById(int id)
    {
        try
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound(ApiResponse<CourseDto>.ErrorResponse($"Course with ID {id} not found"));
            }

            return Ok(ApiResponse<CourseDto>.SuccessResponse(course));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving course with ID {CourseId}", id);
            return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred while retrieving the course"));
        }
    }

    /// <summary>
    /// Create a new course
    /// </summary>
    /// <param name="createCourseDto">Course creation data</param>
    /// <returns>Created course</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        try
        {
            var course = await _courseService.CreateCourseAsync(createCourseDto);
            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = course.Id },
                ApiResponse<CourseDto>.SuccessResponse(course, "Course created successfully"));
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error while creating course");
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating course");
            return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred while creating the course"));
        }
    }

    /// <summary>
    /// Update an existing course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <param name="updateCourseDto">Course update data</param>
    /// <returns>Updated course</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourseDto)
    {
        try
        {
            var course = await _courseService.UpdateCourseAsync(id, updateCourseDto);
            if (course == null)
            {
                return NotFound(ApiResponse<CourseDto>.ErrorResponse($"Course with ID {id} not found"));
            }

            return Ok(ApiResponse<CourseDto>.SuccessResponse(course, "Course updated successfully"));
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error while updating course with ID {CourseId}", id);
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
        }
        catch (BusinessRuleViolationException ex)
        {
            _logger.LogWarning(ex, "Business rule violation while updating course with ID {CourseId}", id);
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating course with ID {CourseId}", id);
            return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred while updating the course"));
        }
    }

    /// <summary>
    /// Delete a course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(int id)
    {
        try
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse($"Course with ID {id} not found"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Course deleted successfully"));
        }
        catch (BusinessRuleViolationException ex)
        {
            _logger.LogWarning(ex, "Business rule violation while deleting course with ID {CourseId}", id);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting course with ID {CourseId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the course"));
        }
    }
}
