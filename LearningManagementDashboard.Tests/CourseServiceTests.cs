using AutoMapper;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Exceptions;
using LearningManagementDashboard.Api.Mappings;
using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Services;
using Moq;

namespace LearningManagementDashboard.Tests;

/// <summary>
/// Unit tests for CourseService with AutoMapper integration
/// </summary>
public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly Mock<IEnrollmentRepository> _mockEnrollmentRepository;
    private readonly IMapper _mapper;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockCourseRepository = new Mock<ICourseRepository>();
        _mockEnrollmentRepository = new Mock<IEnrollmentRepository>();
        
        // Configure AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CourseProfile());
        });
        _mapper = config.CreateMapper();
        
        _courseService = new CourseService(_mockCourseRepository.Object, _mockEnrollmentRepository.Object, _mapper);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ShouldReturnAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            new Course { Id = 1, Name = "Course 1", Description = "Description 1" },
            new Course { Id = 2, Name = "Course 2", Description = "Description 2" }
        };

        _mockCourseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(courses);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(It.IsAny<int>())).ReturnsAsync(5);

        // Act
        var result = await _courseService.GetAllCoursesAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Course 1", result.First().Name);
        Assert.Equal(5, result.First().EnrolledStudents);
    }

    [Fact]
    public async Task GetCourseByIdAsync_ExistingCourse_ShouldReturnCourse()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course", Description = "Test Description" };
        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(10);

        // Act
        var result = await _courseService.GetCourseByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Course", result.Name);
        Assert.Equal("Test Description", result.Description);
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
    public async Task CreateCourseAsync_ValidCourse_ShouldCreateCourse()
    {
        // Arrange
        var createDto = new CreateCourseDto
        {
            Name = "New Course",
            Description = "New Course Description"
        };

        var createdCourse = new Course
        {
            Id = 1,
            Name = createDto.Name,
            Description = createDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _mockCourseRepository.Setup(x => x.CreateAsync(It.IsAny<Course>())).ReturnsAsync(createdCourse);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(0);

        // Act
        var result = await _courseService.CreateCourseAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Course", result.Name);
        Assert.Equal("New Course Description", result.Description);
        Assert.Equal(0, result.EnrolledStudents);
    }

    [Fact]
    public async Task UpdateCourseAsync_ValidCourse_ShouldUpdateCourse()
    {
        // Arrange
        var existingCourse = new Course
        {
            Id = 1,
            Name = "Old Name",
            Description = "Old Description",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var updateDto = new UpdateCourseDto
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        var updatedCourse = new Course
        {
            Id = 1,
            Name = updateDto.Name,
            Description = updateDto.Description,
            CreatedAt = existingCourse.CreatedAt
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(existingCourse);
        _mockCourseRepository.Setup(x => x.UpdateAsync(It.IsAny<Course>())).ReturnsAsync(updatedCourse);
        _mockEnrollmentRepository.Setup(x => x.GetEnrollmentCountByCourseAsync(1)).ReturnsAsync(5);

        // Act
        var result = await _courseService.UpdateCourseAsync(1, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal(5, result.EnrolledStudents);
    }

    [Fact]
    public async Task UpdateCourseAsync_NonExistingCourse_ShouldReturnNull()
    {
        // Arrange
        var updateDto = new UpdateCourseDto
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        _mockCourseRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        // Act
        var result = await _courseService.UpdateCourseAsync(999, updateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_ExistingCourse_ShouldReturnTrue()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Test Course", Description = "Test Description" };
        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetByCourseIdAsync(1)).ReturnsAsync(new List<Enrollment>());
        _mockCourseRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _courseService.DeleteCourseAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCourseAsync_NonExistingCourse_ShouldThrowNotFoundException()
    {
        // Arrange
        _mockCourseRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Course?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _courseService.DeleteCourseAsync(999));
    }

    [Fact]
    public async Task DeleteCourseAsync_CourseWithEnrollments_ShouldThrowConflictException()
    {
        // Arrange
        var course = new Course { Id = 1, Name = "Course 1", Description = "Description 1" };
        var enrollments = new List<Enrollment> { new Enrollment { Id = 1, CourseId = 1, StudentId = 1 } };
        
        _mockCourseRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(course);
        _mockEnrollmentRepository.Setup(x => x.GetByCourseIdAsync(1)).ReturnsAsync(enrollments);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _courseService.DeleteCourseAsync(1));
    }
}
