using FluentValidation;
using LearningManagementDashboard.Api.DTOs;

namespace LearningManagementDashboard.Api.Validators;

/// <summary>
/// Validator for CreateCourseDto to ensure data integrity and business rules
/// </summary>
public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Course name is required")
            .Length(3, 100)
            .WithMessage("Course name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Course description is required")
            .Length(10, 1000)
            .WithMessage("Course description must be between 10 and 1000 characters");
    }
}

/// <summary>
/// Validator for UpdateCourseDto to ensure data integrity and business rules
/// </summary>
public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Course name is required")
            .Length(3, 100)
            .WithMessage("Course name must be between 3 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Course description is required")
            .Length(10, 1000)
            .WithMessage("Course description must be between 10 and 1000 characters");
    }
}
