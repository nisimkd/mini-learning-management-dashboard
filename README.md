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
├── Controllers/          # API Controllers with FluentValidation
├── Services/            # Business logic with AutoMapper integration
│   └── Interfaces/      # Service contracts
├── Repositories/        # Data access with seed data
│   └── Interfaces/      # Repository contracts
├── Models/             # Domain models
├── DTOs/               # Data transfer objects with validation
├── Validators/         # FluentValidation validators
├── Mappings/           # AutoMapper profiles
├── Middleware/         # Custom middleware (Exception, Security)
├── Constants/          # Centralized constants and configuration
├── Exceptions/         # Custom exception classes
├── Program.cs          # Application entry point with DI
└── appsettings.json    # Configuration with CORS settings

src/                    # Frontend (renamed from frontend)
├── src/
│   ├── components/     # Reusable UI components
│   │   ├── layout/     # Layout components
│   │   ├── courses/    # Course-related components
│   │   └── enrollments/ # Enrollment-related components
│   ├── pages/          # Main application pages
│   ├── services/       # API communication with axios
│   │   ├── api.ts      # Main API service
│   │   └── toast.ts    # Toast notification service
│   ├── hooks/          # Custom React hooks
│   │   ├── useAsync.ts # Generic async operations
│   │   ├── useCourses.ts # Course-specific hooks
│   │   ├── useStudents.ts # Student-specific hooks
│   │   └── useEnrollments.ts # Enrollment-specific hooks
│   ├── types/          # TypeScript type definitions
│   └── App.tsx         # Main application component
├── public/             # Static assets
├── package.json        # Frontend dependencies
└── .env.local         # Environment configuration
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
   cd src
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Create environment configuration:
   ```bash
   # Create .env.local file with:
   REACT_APP_API_BASE_URL=http://localhost:5091/api
   REACT_APP_ENVIRONMENT=development
   ```

4. Start the development server:
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

### Enhanced User Experience
- **Toast Notifications** - Real-time feedback for all user actions
- **Loading States** - Visual feedback during API operations
- **Error Handling** - Graceful error boundaries and user-friendly messages
- **Responsive Design** - Optimized for mobile, tablet, and desktop

### Dashboard
- Real-time statistics (active courses, students, enrollments)
- Recent enrollment activity
- Quick action buttons for common tasks
- Visual progress indicators

### Course Management
- **Robust Validation** - FluentValidation for all inputs
- **AutoMapper Integration** - Clean data transformation
- Create new courses with comprehensive validation
- Edit existing courses with capacity management
- Delete courses with enrollment dependency checking
- View course details and enrollment status

### Enrollment Management
- **Business Rule Enforcement** - Prevent duplicate enrollments
- **Exception Handling** - Custom exceptions for business conflicts
- Enroll students in courses with validation
- View enrollment status and tracking
- Remove enrollments with proper cleanup
- Track enrollment history and analytics

### Advanced API Features
- **Global Exception Middleware** - Consistent error responses
- **Security Headers** - Enhanced security with proper HTTP headers
- **CORS Configuration** - Flexible cross-origin resource sharing
- **Comprehensive Logging** - Structured logging throughout
- **RESTful Design** - Industry-standard API patterns

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
- **AutoMapper 12.0.1** - Object-to-object mapping
- **FluentValidation 11.9.0** - Input validation with fluent interface
- **In-memory storage** - Data persistence with seed data
- **Global Exception Middleware** - Centralized error handling
- **Security Headers Middleware** - Enhanced security
- **CORS Configuration** - Cross-origin resource sharing
- **OpenAPI/Swagger** - API documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework for tests

### Frontend
- **React 19** - Frontend framework
- **TypeScript** - Type-safe JavaScript
- **React Router 7.6.3** - Client-side routing
- **Axios 1.10.0** - HTTP client for API communication
- **React-Toastify 11.0.5** - Toast notifications for UX
- **Custom Hooks** - Reusable stateful logic
- **Environment Configuration** - .env.local setup
- **CSS3** - Modern styling with flexbox and grid

### Development Tools
- **VS Code Tasks** - Automated build, run, and test tasks
- **ESLint & TypeScript** - Code quality and type safety
- **Hot Reload** - Development server with live updates
- **Comprehensive Documentation** - README with setup guides

## Professional Development Practices

This project demonstrates enterprise-level development practices:

### Backend Architecture
- **Clean Architecture** with clear separation of concerns
- **Dependency Injection** for loose coupling and testability
- **AutoMapper Integration** for clean object mapping
- **FluentValidation** for robust input validation
- **Global Exception Handling** with custom middleware
- **Security Best Practices** with headers and CORS
- **Centralized Constants** eliminating magic strings
- **Custom Exceptions** for business rule enforcement

### Frontend Architecture  
- **Custom Hooks Pattern** for reusable stateful logic
- **Environment Configuration** for different deployment stages
- **Toast Notifications** for enhanced user experience
- **Error Boundaries** for graceful error handling
- **Type Safety** with comprehensive TypeScript usage
- **Responsive Design** for mobile and desktop compatibility

### Development Workflow
- **Comprehensive Unit Testing** with 9 passing tests
- **VS Code Integration** with automated tasks
- **RESTful API Design** following industry standards
- **Component-Based Architecture** for maintainability
- **State Management** with React hooks and custom patterns
- **Professional UI/UX** with modern design principles
- **Code Quality** with consistent styling and documentation

### DevOps & Tooling
- **Automated Build Tasks** for backend and frontend
- **Environment-Specific Configuration** 
- **Package Management** with version control
- **Documentation** with comprehensive setup guides
- **Testing Strategy** with mocking and integration tests
