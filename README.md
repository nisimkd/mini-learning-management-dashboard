# Learning Management Dashboard

A mini-Learning Management Dashboard for an online education platform, built with .NET 9 Web API and React.

## Project Overview

This dashboard allows administrators to:
- View a list of student enrollments
- Add and edit courses
- Assign students to courses
- Generate simple enrollment reports

## Architecture

The project follows clean architecture principles with proper separation of concerns:

### Backend (.NET 9 Web API)
- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic layer
- **Repositories**: Data access layer with in-memory storage
- **Models**: Domain entities (Student, Course, Enrollment)
- **DTOs**: Data transfer objects for API communication
- **Exceptions**: Custom exception classes for error handling

### Project Structure
```
LearningManagementDashboard.Api/
├── Controllers/          # API Controllers
├── Services/            # Business logic
│   └── Interfaces/      # Service contracts
├── Repositories/        # Data access
│   └── Interfaces/      # Repository contracts
├── Models/             # Domain models
├── DTOs/               # Data transfer objects
├── Exceptions/         # Custom exceptions
├── Program.cs          # Application entry point
└── appsettings.json    # Configuration
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio Code or Visual Studio

### Running the API
1. Navigate to the API project directory:
   ```bash
   cd LearningManagementDashboard.Api
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or the port specified in launchSettings.json).

## API Endpoints (Planned)

### Courses
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create a new course
- `PUT /api/courses/{id}` - Update a course
- `DELETE /api/courses/{id}` - Delete a course

### Enrollments
- `GET /api/enrollments` - Get all enrollments
- `GET /api/enrollments/{id}` - Get enrollment by ID
- `POST /api/enrollments` - Create a new enrollment
- `DELETE /api/enrollments/{id}` - Delete an enrollment
- `GET /api/enrollments/report` - Generate enrollment report

## Development Status

- [x] Phase 1: Project setup and architecture
- [ ] Phase 2: Backend implementation
- [ ] Phase 3: Frontend foundation
- [ ] Phase 4: UI implementation
- [ ] Phase 5: Integration and reporting
- [ ] Phase 6: Finalization

## Technologies Used

- **.NET 9** - Web API framework
- **ASP.NET Core** - Web framework
- **In-memory storage** - Data persistence
- **OpenAPI/Swagger** - API documentation
- **React** - Frontend framework (planned)

## Contributing

This project demonstrates professional development practices including:
- Clean architecture with separation of concerns
- Dependency injection
- Proper error handling
- Unit testing
- RESTful API design
- Comprehensive documentation
