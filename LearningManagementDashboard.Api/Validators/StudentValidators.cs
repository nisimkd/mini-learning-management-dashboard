using FluentValidation;
using LearningManagementDashboard.Api.Constants;
using LearningManagementDashboard.Api.DTOs;

namespace LearningManagementDashboard.Api.Validators;

/// <summary>
/// Validation rules for student-related DTOs
/// </summary>
public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Student full name is required")
            .Length(ApiConstants.Validation.MinStudentNameLength, ApiConstants.Validation.MaxStudentNameLength)
            .WithMessage($"Student name must be between {ApiConstants.Validation.MinStudentNameLength} and {ApiConstants.Validation.MaxStudentNameLength} characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .Length(ApiConstants.Validation.MinEmailLength, ApiConstants.Validation.MaxEmailLength)
            .WithMessage($"Email must be between {ApiConstants.Validation.MinEmailLength} and {ApiConstants.Validation.MaxEmailLength} characters");
    }
}

/// <summary>
/// Validation rules for updating student DTOs
/// </summary>
public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
{
    public UpdateStudentDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Student full name is required")
            .Length(ApiConstants.Validation.MinStudentNameLength, ApiConstants.Validation.MaxStudentNameLength)
            .WithMessage($"Student name must be between {ApiConstants.Validation.MinStudentNameLength} and {ApiConstants.Validation.MaxStudentNameLength} characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .Length(ApiConstants.Validation.MinEmailLength, ApiConstants.Validation.MaxEmailLength)
            .WithMessage($"Email must be between {ApiConstants.Validation.MinEmailLength} and {ApiConstants.Validation.MaxEmailLength} characters");
    }
}
