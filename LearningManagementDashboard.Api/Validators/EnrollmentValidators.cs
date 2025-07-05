using FluentValidation;
using LearningManagementDashboard.Api.DTOs;
using LearningManagementDashboard.Api.Constants;

namespace LearningManagementDashboard.Api.Validators;

/// <summary>
/// Validator for CreateEnrollmentDto to ensure data integrity and business rules
/// </summary>
public class CreateEnrollmentDtoValidator : AbstractValidator<CreateEnrollmentDto>
{
    public CreateEnrollmentDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be a positive number");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be a positive number");
    }
}
