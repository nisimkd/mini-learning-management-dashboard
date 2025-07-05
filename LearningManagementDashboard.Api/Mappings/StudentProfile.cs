using AutoMapper;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Mappings;

/// <summary>
/// AutoMapper profile for mapping between Student entities and DTOs
/// </summary>
public class StudentProfile : Profile
{
    public StudentProfile()
    {
        // Student to StudentDto mapping
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.EnrolledCourses, opt => opt.Ignore()); // Set by service layer
    }
}
