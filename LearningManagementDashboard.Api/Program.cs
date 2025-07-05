var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS policy for frontend integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // React development server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register repositories
builder.Services.AddSingleton<LearningManagementDashboard.Api.Repositories.Interfaces.ICourseRepository, LearningManagementDashboard.Api.Repositories.InMemoryCourseRepository>();
builder.Services.AddSingleton<LearningManagementDashboard.Api.Repositories.Interfaces.IStudentRepository, LearningManagementDashboard.Api.Repositories.InMemoryStudentRepository>();
builder.Services.AddSingleton<LearningManagementDashboard.Api.Repositories.Interfaces.IEnrollmentRepository, LearningManagementDashboard.Api.Repositories.InMemoryEnrollmentRepository>();

// Register services
builder.Services.AddScoped<LearningManagementDashboard.Api.Services.Interfaces.ICourseService, LearningManagementDashboard.Api.Services.CourseService>();
builder.Services.AddScoped<LearningManagementDashboard.Api.Services.Interfaces.IEnrollmentService, LearningManagementDashboard.Api.Services.EnrollmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");

// Map controllers
app.MapControllers();

app.Run();
