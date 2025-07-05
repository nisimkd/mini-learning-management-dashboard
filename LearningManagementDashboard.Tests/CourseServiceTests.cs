using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services;
using Moq;

namespace LearningManagementDashboard.Tests;

/// <summary>
/// Unit tests for CourseService
/// </summary>
public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly Mock<IEnrollmentRepository> _mockEnrollmentRepository;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockCourseRepository = new Mock<ICourseRepository>();
        _mockEnrollmentRepository = new Mock<IEnrollmentRepository>();
        _courseService = new CourseService(_mockCourseRepository.Object, _mockEnrollmentRepository.Object);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ShouldReturnAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, Title = "Course 1", Code = "C1", MaxCapacity = 20 },
            new Course { Id = 2, Title = "Course 2", Code = "C2", MaxCapacity = 30 }
        };

        _mockCourseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(courses);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(It.IsAny<int>())).ReturnsAsync(5);

        // Act
        var result = await _courseService.GetAllCoursesAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Course 1", result.First().Title);
        Assert.Equal(5, result.First().EnrolledStudents);
    }

    [Fact]
    public async Task GetCourseByIdAsync_ExistingCourse_ShouldReturnCourse()
    {
        // Arrange
        var course = new Course { Id = 1, Title = "Test Course", Code = "TC1", MaxCapacity = 25 };
        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(10);

        // Act
        var result = await _courseService.GetCourseByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Course", result.Title);
        Assert.Equal(10, result.EnrolledStudents);
    }

    [Fact]
    public async Task GetCourseByIdAsync_NonExistingCourse_ShouldReturnNull()
    {
        // Arrange
        _mockCourseRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.GetCourseByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateCourseAsync_ValidData_ShouldReturnCreatedCourse()
    {
        // Arrange
        var createDto = new CreateCourseDto
        {
            Title = "New Course",
            Description = "Test Description",
            Code = "NC1",
            MaxCapacity = 20
        };

        var createdCourse = new Course
        {
            Id = 1,
            Title = createDto.Title,
            Description = createDto.Description,
            Code = createDto.Code.ToUpper(),
            MaxCapacity = createDto.MaxCapacity,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mockCourseRepository.Setup(x => x.CreateAsync(It.IsAny<Course>())).ReturnsAsync(createdCourse);

        // Act
        var result = await _courseService.CreateCourseAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Course", result.Title);
        Assert.Equal("NC1", result.Code);
        Assert.Equal(20, result.MaxCapacity);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateCourseAsync_EmptyTitle_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateCourseDto
        {
            Title = "",
            Description = "Test Description",
            Code = "NC1",
            MaxCapacity = 20
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _courseService.CreateCourseAsync(createDto));
    }

    [Fact]
    public async Task CreateCourseAsync_InvalidCapacity_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateCourseDto
        {
            Title = "New Course",
            Description = "Test Description",
            Code = "NC1",
            MaxCapacity = 0
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _courseService.CreateCourseAsync(createDto));
    }

    [Fact]
    public async Task UpdateCourseAsync_ValidData_ShouldReturnUpdatedCourse()
    {
        // Arrange
        var existingCourse = new Course
        {
            Id = 1,
            Title = "Old Title",
            Code = "OT1",
            MaxCapacity = 20,
            IsActive = true
        };

        var updateDto = new UpdateCourseDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Code = "UT1",
            MaxCapacity = 25,
            IsActive = true
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingCourse);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(10);
        _mockCourseRepository.Setup(x => x.UpdateAsync(It.IsAny<Course>())).ReturnsAsync(existingCourse);

        // Act
        var result = await _courseService.UpdateCourseAsync(1, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("UT1", result.Code);
    }

    [Fact]
    public async Task UpdateCourseAsync_ReduceCapacityBelowEnrollments_ShouldThrowBusinessRuleViolationException()
    {
        // Arrange
        var existingCourse = new Course
        {
            Id = 1,
            Title = "Test Course",
            Code = "TC1",
            MaxCapacity = 30,
            IsActive = true
        };

        var updateDto = new UpdateCourseDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            Code = "UT1",
            MaxCapacity = 5, // Reducing below current enrollments
            IsActive = true
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingCourse);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(10); // 10 students enrolled

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleViolationException>(() => _courseService.UpdateCourseAsync(1, updateDto));
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseWithActiveEnrollments_ShouldThrowBusinessRuleViolationException()
    {
        // Arrange
        var course = new Course { Id = 1, Title = "Test Course", Code = "TC1" };
        var activeEnrollments = new List<Enrollment>
        {
            new Enrollment { Id = 1, CourseId = 1, StudentId = 1, Status = EnrollmentStatus.Active }
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetByCourseIdAsync(1)).ReturnsAsync(activeEnrollments);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleViolationException>(() => _courseService.DeleteCourseAsync(1));
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseWithNoActiveEnrollments_ShouldReturnTrue()
    {
        // Arrange
        var course = new Course { Id = 1, Title = "Test Course", Code = "TC1" };
        var inactiveEnrollments = new List<Enrollment>
        {
            new Enrollment { Id = 1, CourseId = 1, StudentId = 1, Status = EnrollmentStatus.Completed }
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetByCourseIdAsync(1)).ReturnsAsync(inactiveEnrollments);
        _mockCourseRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _courseService.DeleteCourseAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_NonExistingCourse_ShouldReturnFalse()
    {
        // Arrange
        _mockCourseRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.DeleteCourseAsync(999);

        // Assert
        Assert.False(result);
    }
}
