using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace LearningManagementDashboard.Api.Repositories;

/// <summary>
/// In-memory implementation of the enrollment repository
/// </summary>
public class InMemoryEnrollmentRepository : IEnrollmentRepository
{
    private readonly ConcurrentDictionary<int, Enrollment> _enrollments = new();
    private int _nextId = 1;

    public InMemoryEnrollmentRepository()
    {
        // Seed with sample data
        SeedSampleData();
    }

    public Task<IEnumerable<Enrollment>> GetAllAsync()
    {
        return Task.FromResult(_enrollments.Values.AsEnumerable());
    }

    public Task<Enrollment?> GetByIdAsync(int id)
    {
        _enrollments.TryGetValue(id, out var enrollment);
        return Task.FromResult(enrollment);
    }

    public Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        var enrollments = _enrollments.Values.Where(e => e.StudentId == studentId);
        return Task.FromResult(enrollments);
    }

    public Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseId)
    {
        var enrollments = _enrollments.Values.Where(e => e.CourseId == courseId);
        return Task.FromResult(enrollments);
    }

    public Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        enrollment.Id = _nextId++;
        enrollment.EnrollmentDate = DateTime.UtcNow;
        
        _enrollments.TryAdd(enrollment.Id, enrollment);
        return Task.FromResult(enrollment);
    }

    public Task<Enrollment> UpdateAsync(Enrollment enrollment)
    {
        _enrollments.TryUpdate(enrollment.Id, enrollment, _enrollments[enrollment.Id]);
        return Task.FromResult(enrollment);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_enrollments.TryRemove(id, out _));
    }

    public Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
    {
        var isEnrolled = _enrollments.Values.Any(e => 
            e.StudentId == studentId && 
            e.CourseId == courseId && 
            e.Status == EnrollmentStatus.Active);
        
        return Task.FromResult(isEnrolled);
    }

    public Task<int> GetEnrollmentCountByCourseAsync(int courseId)
    {
        var count = _enrollments.Values.Count(e => 
            e.CourseId == courseId && 
            e.Status == EnrollmentStatus.Active);
        
        return Task.FromResult(count);
    }

    private void SeedSampleData()
    {
        var enrollments = new[]
        {
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 1,
                CourseId = 1,
                EnrollmentDate = DateTime.UtcNow.AddDays(-10),
                Status = EnrollmentStatus.Active
            },
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 2,
                CourseId = 1,
                EnrollmentDate = DateTime.UtcNow.AddDays(-8),
                Status = EnrollmentStatus.Active
            },
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 3,
                CourseId = 2,
                EnrollmentDate = DateTime.UtcNow.AddDays(-7),
                Status = EnrollmentStatus.Active
            },
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 1,
                CourseId = 2,
                EnrollmentDate = DateTime.UtcNow.AddDays(-6),
                Status = EnrollmentStatus.Active
            },
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 4,
                CourseId = 3,
                EnrollmentDate = DateTime.UtcNow.AddDays(-5),
                Status = EnrollmentStatus.Active
            },
            new Enrollment
            {
                Id = _nextId++,
                StudentId = 5,
                CourseId = 1,
                EnrollmentDate = DateTime.UtcNow.AddDays(-4),
                Status = EnrollmentStatus.Completed
            }
        };

        foreach (var enrollment in enrollments)
        {
            _enrollments.TryAdd(enrollment.Id, enrollment);
        }
    }
}
