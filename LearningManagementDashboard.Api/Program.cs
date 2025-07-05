using FluentValidation;
using FluentValidation.AspNetCore;
using LearningManagementDashboard.Api.Middleware;
using LearningManagementDashboard.Api.Mappings;
using LearningManagementDashboard.Api.Validators;
using LearningManagementDashboard.Api.Repositories.Interfaces;
using LearningManagementDashboard.Api.Repositories;
using LearningManagementDashboard.Api.Services.Interfaces;
using LearningManagementDashboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseDtoValidator>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(CourseProfile), typeof(StudentProfile), typeof(EnrollmentProfile));

// Configure CORS with settings from appsettings.json
var corsAllowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(corsAllowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register repositories as singletons (in-memory data)
builder.Services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
builder.Services.AddSingleton<IEnrollmentRepository, InMemoryEnrollmentRepository>();

// Register services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

// Configure the HTTP request pipeline

// Add security headers middleware
app.UseMiddleware<SecurityHeadersMiddleware>();

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Note: Removed UseHttpsRedirection for local development
// app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");

// Map controllers
app.MapControllers();

app.Run();
