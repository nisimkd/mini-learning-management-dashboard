namespace LearningManagementDashboard.Api.Controllers;

/// <summary>
/// API controller for enrollment operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(IEnrollmentService enrollmentService, ILogger<EnrollmentsController> logger)
    {
        _enrollmentService = enrollmentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all enrollments
    /// </summary>
    /// <returns>List of enrollments</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentDto>>>> GetAllEnrollments()
    {
        try
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(ApiResponse<IEnumerable<EnrollmentDto>>.SuccessResponse(enrollments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all enrollments");
            return StatusCode(500, ApiResponse<IEnumerable<EnrollmentDto>>.ErrorResponse("An error occurred while retrieving enrollments"));
        }
    }

    /// <summary>
    /// Get an enrollment by ID
    /// </summary>
    /// <param name="id">Enrollment ID</param>
    /// <returns>Enrollment details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> GetEnrollmentById(int id)
    {
        try
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            if (enrollment == null)
            {
                return NotFound(ApiResponse<EnrollmentDto>.ErrorResponse($"Enrollment with ID {id} not found"));
            }

            return Ok(ApiResponse<EnrollmentDto>.SuccessResponse(enrollment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving enrollment with ID {EnrollmentId}", id);
            return StatusCode(500, ApiResponse<EnrollmentDto>.ErrorResponse("An error occurred while retrieving the enrollment"));
        }
    }

    /// <summary>
    /// Create a new enrollment (assign a student to a course)
    /// </summary>
    /// <param name="createEnrollmentDto">Enrollment creation data</param>
    /// <returns>Created enrollment</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> CreateEnrollment([FromBody] CreateEnrollmentDto createEnrollmentDto)
    {
        try
        {
            var enrollment = await _enrollmentService.CreateEnrollmentAsync(createEnrollmentDto);
            return CreatedAtAction(
                nameof(GetEnrollmentById),
                new { id = enrollment.Id },
                ApiResponse<EnrollmentDto>.SuccessResponse(enrollment, "Student enrolled successfully"));
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Entity not found while creating enrollment");
            return NotFound(ApiResponse<EnrollmentDto>.ErrorResponse(ex.Message));
        }
        catch (BusinessRuleViolationException ex)
        {
            _logger.LogWarning(ex, "Business rule violation while creating enrollment");
            return BadRequest(ApiResponse<EnrollmentDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating enrollment");
            return StatusCode(500, ApiResponse<EnrollmentDto>.ErrorResponse("An error occurred while creating the enrollment"));
        }
    }

    /// <summary>
    /// Delete an enrollment
    /// </summary>
    /// <param name="id">Enrollment ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteEnrollment(int id)
    {
        try
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse($"Enrollment with ID {id} not found"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Enrollment deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting enrollment with ID {EnrollmentId}", id);
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting the enrollment"));
        }
    }

    /// <summary>
    /// Generate enrollment report
    /// </summary>
    /// <returns>Enrollment report data</returns>
    [HttpGet("report")]
    public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentReportDto>>>> GenerateEnrollmentReport()
    {
        try
        {
            var report = await _enrollmentService.GenerateEnrollmentReportAsync();
            return Ok(ApiResponse<IEnumerable<EnrollmentReportDto>>.SuccessResponse(report));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating enrollment report");
            return StatusCode(500, ApiResponse<IEnumerable<EnrollmentReportDto>>.ErrorResponse("An error occurred while generating the report"));
        }
    }
}
