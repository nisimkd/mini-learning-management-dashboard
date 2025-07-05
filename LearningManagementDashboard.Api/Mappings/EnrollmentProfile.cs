using AutoMapper;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Models;

namespace LearningManagementDashboard.Api.Mappings;

/// <summary>
/// AutoMapper profile for mapping between Enrollment entities and DTOs
/// </summary>
public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        // Enrollment to EnrollmentDto mapping
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(dest => dest.StudentName, opt => opt.Ignore()) // Set by service layer
            .ForMember(dest => dest.StudentEmail, opt => opt.Ignore()) // Set by service layer
            .ForMember(dest => dest.CourseName, opt => opt.Ignore()); // Set by service layer

        // CreateEnrollmentDto to Enrollment mapping
        CreateMap<CreateEnrollmentDto, Enrollment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
