using FluentValidation;
using LearningManagementDashboard.Api.Constants;
using LearningManagementDashboard.Api.DTOs;
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
    private readonly IValidator<CreateCourseDto> _createCourseValidator;
    private readonly IValidator<UpdateCourseDto> _updateCourseValidator;

    public CoursesController(
        ICourseService courseService, 
        ILogger<CoursesController> logger,
        IValidator<CreateCourseDto> createCourseValidator,
        IValidator<UpdateCourseDto> updateCourseValidator)
    {
        _courseService = courseService;
        _logger = logger;
        _createCourseValidator = createCourseValidator;
        _updateCourseValidator = updateCourseValidator;
    }

    /// <summary>
    /// Get all courses
    /// </summary>
    /// <returns>List of courses</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(courses, ApiConstants.Messages.Success));
    }

    /// <summary>
    /// Get a course by ID
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourseById(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDto>.ErrorResponse(ApiConstants.Messages.CourseNotFound));
        }

        return Ok(ApiResponse<CourseDto>.SuccessResponse(course, ApiConstants.Messages.CourseRetrievedSuccessfully));
    }

    /// <summary>
    /// Create a new course
    /// </summary>
    /// <param name="createCourseDto">Course creation data</param>
    /// <returns>Created course</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
    {
        // Validate the request
        var validationResult = await _createCourseValidator.ValidateAsync(createCourseDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var course = await _courseService.CreateCourseAsync(createCourseDto);
        return CreatedAtAction(
            nameof(GetCourseById),
            new { id = course.Id },
            ApiResponse<CourseDto>.SuccessResponse(course, ApiConstants.Messages.CourseCreatedSuccessfully));
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
        // Validate the request
        var validationResult = await _updateCourseValidator.ValidateAsync(updateCourseDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<CourseDto>.ErrorResponse(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var course = await _courseService.UpdateCourseAsync(id, updateCourseDto);
        if (course == null)
        {
            return NotFound(ApiResponse<CourseDto>.ErrorResponse(ApiConstants.Messages.CourseNotFound));
        }

        return Ok(ApiResponse<CourseDto>.SuccessResponse(course, ApiConstants.Messages.CourseUpdatedSuccessfully));
    }

    /// <summary>
    /// Delete a course
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(int id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        return Ok(ApiResponse<bool>.SuccessResponse(result, ApiConstants.Messages.CourseDeletedSuccessfully));
    }
}
