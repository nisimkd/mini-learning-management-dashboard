# Learning Management Dashboard

A comprehensive Learning Management Dashboard for an online education platform, built with .NET 9 Web API and React with TypeScript.

## Project Overview

This dashboard allows administrators to:
- View a comprehensive dashboard with statistics and recent activity
- Manage courses (create, edit, delete, view details)
- Manage student enrollments with course assignments
- Generate detailed enrollment reports with analytics
- View course utilization and student progress

## Architecture

The project follows clean architecture principles with proper separation of concerns:

### Backend (.NET 9 Web API)
- **Controllers**: Handle HTTP requests and responses with comprehensive error handling
- **Services**: Business logic layer with validation and business rules
- **Repositories**: Data access layer with in-memory storage and seed data
- **Models**: Domain entities (Student, Course, Enrollment)
- **DTOs**: Data transfer objects for API communication
- **Exceptions**: Custom exception classes for error handling

### Frontend (React + TypeScript)
- **Components**: Reusable UI components with proper TypeScript typing
- **Pages**: Main application views (Dashboard, Courses, Enrollments, Reports)
- **Services**: API communication layer with axios
- **Types**: TypeScript interfaces matching backend models
- **Routing**: React Router for navigation and route management

### Project Structure
```
LearningManagementDashboard.Api/
├── Controllers/          # API Controllers (Courses, Enrollments, Students)
├── Services/            # Business logic with validation
│   └── Interfaces/      # Service contracts
├── Repositories/        # Data access with seed data
│   └── Interfaces/      # Repository contracts
├── Models/             # Domain models
├── DTOs/               # Data transfer objects
├── Exceptions/         # Custom exceptions
├── Program.cs          # Application entry point with DI
└── appsettings.json    # Configuration

frontend/
├── src/
│   ├── components/     # Reusable UI components
│   │   ├── layout/     # Layout components
│   │   ├── courses/    # Course-related components
│   │   └── enrollments/ # Enrollment-related components
│   ├── pages/          # Main application pages
│   ├── services/       # API communication
│   ├── types/          # TypeScript type definitions
│   └── App.tsx         # Main application component
├── public/             # Static assets
└── package.json        # Frontend dependencies
```

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js (16+ recommended)
- Visual Studio Code or Visual Studio

### Running the Backend API
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

The API will be available at `http://localhost:5091`.

### Running the Frontend
1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

The frontend will be available at `http://localhost:3000`.

### Running Tests
1. Navigate to the test project directory:
   ```bash
   cd LearningManagementDashboard.Tests
   ```

2. Run the tests:
   ```bash
   dotnet test
   ```

## API Endpoints

### Courses
- `GET /api/courses` - Get all courses with enrollment counts
- `GET /api/courses/{id}` - Get course by ID with details
- `POST /api/courses` - Create a new course
- `PUT /api/courses/{id}` - Update a course
- `DELETE /api/courses/{id}` - Delete a course (with validation)

### Students
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID

### Enrollments
- `GET /api/enrollments` - Get all enrollments with student/course data
- `GET /api/enrollments/{id}` - Get enrollment by ID
- `POST /api/enrollments` - Create a new enrollment
- `DELETE /api/enrollments/{id}` - Delete an enrollment
- `GET /api/enrollments/report` - Generate comprehensive enrollment report

## Features

### Dashboard
- Real-time statistics (active courses, students, enrollments)
- Recent enrollment activity
- Quick action buttons for common tasks

### Course Management
- Create new courses with validation
- Edit existing courses with capacity management
- Delete courses with enrollment checking
- View course details and enrollment status

### Enrollment Management
- Enroll students in courses
- View enrollment status and grades
- Remove enrollments
- Track enrollment history

### Reports
- Course utilization analysis
- Student enrollment summaries
- Visual progress indicators
- Export capabilities (future enhancement)

## Development Status

- [x] **Phase 1**: Project setup and architecture
- [x] **Phase 2**: Backend implementation with comprehensive API
- [x] **Phase 3**: Frontend foundation with React and routing
- [x] **Phase 4**: Complete UI implementation with forms and reports
- [x] **Phase 5**: Integration and error handling
- [x] **Phase 6**: Testing and finalization

## Technologies Used

### Backend
- **.NET 9** - Web API framework
- **ASP.NET Core** - Web framework
- **In-memory storage** - Data persistence with seed data
- **OpenAPI/Swagger** - API documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework for tests

### Frontend
- **React 19** - Frontend framework
- **TypeScript** - Type-safe JavaScript
- **React Router** - Client-side routing
- **Axios** - HTTP client for API communication
- **CSS3** - Modern styling with flexbox and grid

## Professional Development Practices

This project demonstrates:
- **Clean Architecture** with clear separation of concerns
- **Dependency Injection** for loose coupling
- **Comprehensive Error Handling** with custom exceptions
- **Unit Testing** with high coverage
- **RESTful API Design** following best practices
- **Type Safety** with TypeScript
- **Responsive Design** for mobile and desktop
- **Component-Based Architecture** for maintainability
- **State Management** with React hooks
- **Professional UI/UX** with modern design patterns
