using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace LearningManagementDashboard.Api.Repositories;

/// <summary>
/// In-memory implementation of the course repository
/// </summary>
public class InMemoryCourseRepository : ICourseRepository
{
    private readonly ConcurrentDictionary<int, Course> _courses = new();
    private int _nextId = 1;

    public InMemoryCourseRepository()
    {
        // Seed with sample data
        SeedSampleData();
    }

    public Task<IEnumerable<Course>> GetAllAsync()
    {
        return Task.FromResult(_courses.Values.AsEnumerable());
    }

    public Task<Course?> GetByIdAsync(int id)
    {
        _courses.TryGetValue(id, out var course);
        return Task.FromResult(course);
    }

    public Task<Course> CreateAsync(Course course)
    {
        course.Id = _nextId++;
        course.CreatedAt = DateTime.UtcNow;
        
        _courses.TryAdd(course.Id, course);
        return Task.FromResult(course);
    }

    public Task<Course> UpdateAsync(Course course)
    {
        _courses.TryUpdate(course.Id, course, _courses[course.Id]);
        return Task.FromResult(course);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_courses.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_courses.ContainsKey(id));
    }

    private void SeedSampleData()
    {
        var courses = new[]
        {
            new Course
            {
                Id = _nextId++,
                Name = "Introduction to Programming",
                Description = "Learn the fundamentals of programming with C#",
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Course
            {
                Id = _nextId++,
                Name = "Web Development Basics",
                Description = "Introduction to HTML, CSS, and JavaScript",
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            },
            new Course
            {
                Id = _nextId++,
                Name = "Database Design",
                Description = "Learn database design principles and SQL",
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            }
        };

        foreach (var course in courses)
        {
            _courses.TryAdd(course.Id, course);
        }
    }
}
