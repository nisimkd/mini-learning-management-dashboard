using AutoMapper;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Mappings;

/// <summary>
/// AutoMapper profile for mapping between Course entities and DTOs
/// </summary>
public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // Course to CourseDto mapping
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.EnrolledStudents, opt => opt.Ignore()); // Set by service layer

        // CreateCourseDto to Course mapping
        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // UpdateCourseDto to Course mapping
        CreateMap<UpdateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
