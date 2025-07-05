using LearningManagementDashboard.Api.Models;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace LearningManagementDashboard.Api.Repositories;

/// <summary>
/// In-memory implementation of the student repository
/// </summary>
public class InMemoryStudentRepository : IStudentRepository
{
    private readonly ConcurrentDictionary<int, Student> _students = new();
    private int _nextId = 1;

    public InMemoryStudentRepository()
    {
        // Seed with sample data
        SeedSampleData();
    }

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        return Task.FromResult(_students.Values.AsEnumerable());
    }

    public Task<Student?> GetByIdAsync(int id)
    {
        _students.TryGetValue(id, out var student);
        return Task.FromResult(student);
    }

    public Task<Student> CreateAsync(Student student)
    {
        student.Id = _nextId++;
        student.CreatedAt = DateTime.UtcNow;
        
        _students.TryAdd(student.Id, student);
        return Task.FromResult(student);
    }

    public Task<Student> UpdateAsync(Student student)
    {
        _students.TryUpdate(student.Id, student, _students[student.Id]);
        return Task.FromResult(student);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_students.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_students.ContainsKey(id));
    }

    private void SeedSampleData()
    {
        var students = new[]
        {
            new Student
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new Student
            {
                Id = _nextId++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new Student
            {
                Id = _nextId++,
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Student
            {
                Id = _nextId++,
                FirstName = "Alice",
                LastName = "Williams",
                Email = "alice.williams@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new Student
            {
                Id = _nextId++,
                FirstName = "Charlie",
                LastName = "Brown",
                Email = "charlie.brown@example.com",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            }
        };

        foreach (var student in students)
        {
            _students.TryAdd(student.Id, student);
        }
    }
}
