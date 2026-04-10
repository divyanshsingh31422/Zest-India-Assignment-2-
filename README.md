# Student Management System

A comprehensive Student Management System built with ASP.NET Core Web API and React frontend, featuring JWT authentication, layered architecture, and modern development practices.

## Features

### Backend (.NET Core Web API)
- **JWT Authentication** - Secure token-based authentication
- **Layered Architecture** - Clean separation of concerns (Controller, Service, Repository)
- **Global Exception Handling** - Centralized error handling middleware
- **Serilog Logging** - Structured logging with file and console outputs
- **Swagger Documentation** - Interactive API documentation
- **SQL Server Database** - Entity Framework Core with SQL Server
- **Unit Testing** - Comprehensive test coverage with xUnit and Moq
- **Docker Support** - Containerized deployment ready

### Frontend (React)
- **Modern React** - Functional components with hooks
- **JWT Integration** - Secure API calls with automatic token management
- **Responsive Design** - Mobile-friendly interface
- **CRUD Operations** - Complete student management functionality
- **Routing** - React Router for navigation
- **Axios HTTP Client** - Efficient API communication

## Architecture

### Backend Architecture
```
StudentManagement.Api/
 Controllers/          # API Controllers
   - AuthController.cs
   - StudentsController.cs
 Services/             # Business Logic Layer
   - IAuthService.cs
   - AuthService.cs
   - IStudentService.cs
   - StudentService.cs
 Repositories/         # Data Access Layer
   - IStudentRepository.cs
   - StudentRepository.cs
 Data/                # Database Context
   - StudentDbContext.cs
 Models/              # Data Models
   - Student.cs
   - LoginRequest.cs
   - LoginResponse.cs
   - StudentCreateDto.cs
   - StudentUpdateDto.cs
 Middleware/          # Custom Middleware
   - ExceptionHandlingMiddleware.cs
 Program.cs           # Application Configuration
```

### Frontend Architecture
```
StudentManagement.Frontend/
 src/
  components/         # React Components
    - Login.js
    - Navbar.js
    - StudentList.js
    - StudentForm.js
  services/           # API Services
    - api.js
  App.js              # Main Application Component
  App.css             # Global Styles
  index.js            # Application Entry Point
```

## Database Schema

### Student Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | Primary Key, Identity |
| Name | nvarchar(100) | Required |
| Email | nvarchar(100) | Required, Unique |
| Age | int | Required (18-100) |
| Course | nvarchar(50) | Required |
| CreatedDate | datetime | Default: UTC Now |

## Prerequisites

### For Backend
- .NET 8.0 SDK or later
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

### For Frontend
- Node.js 16+ and npm
- Modern web browser

### For Docker
- Docker Desktop
- Docker Compose

## Quick Start

### Option 1: Docker (Recommended)

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Zest_India_Assignment
   ```

2. **Run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

3. **Access the applications**
   - API: http://localhost:5000
   - Frontend: http://localhost:3000
   - Swagger: http://localhost:5000/swagger

### Option 2: Local Development

#### Backend Setup

1. **Navigate to API project**
   ```bash
   cd StudentManagement.Api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

#### Frontend Setup

1. **Navigate to frontend project** (in a new terminal)
   ```bash
   cd StudentManagement.Frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start the development server**
   ```bash
   npm start
   ```

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `GET /api/auth/test` - Test authentication (requires JWT)

### Students (JWT Required)
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

## Demo Users

The system includes demo users for testing:

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@studentmanagement.com | Admin123! |
| Teacher | teacher@studentmanagement.com | Teacher123! |
| User | user@studentmanagement.com | User123! |

## Testing

### Backend Tests

1. **Run unit tests**
   ```bash
   cd StudentManagement.Tests
   dotnet test
   ```

2. **Run tests with coverage**
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

### Frontend Tests

1. **Run React tests**
   ```bash
   cd StudentManagement.Frontend
   npm test
   ```

## Configuration

### JWT Settings (appsettings.json)
```json
"JwtSettings": {
  "SecretKey": "YourSecretKeyHere",
  "Issuer": "StudentManagementAPI",
  "Audience": "StudentManagementUsers",
  "ExpirationInMinutes": 60
}
```

### Logging Configuration
```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information"
  },
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "logs/student-management-.log" } }
  ]
}
```

## Docker Configuration

### Dockerfile Highlights
- Multi-stage build for optimized image size
- .NET 8.0 runtime base image
- Automatic database creation on startup
- Production-ready configuration

### Docker Compose Services
- **studentmanagement-api**: ASP.NET Core API application
- **sqlserver**: SQL Server 2022 Express with persistent data

## Development Guidelines

### Code Quality
- Follow C# naming conventions
- Use async/await for asynchronous operations
- Implement proper error handling and logging
- Write unit tests for business logic

### Security
- JWT tokens for authentication
- Input validation and sanitization
- SQL injection prevention via Entity Framework
- CORS configuration for cross-origin requests

### Performance
- Efficient database queries with Entity Framework
- Proper indexing on frequently queried columns
- Response caching where appropriate
- Lazy loading for related data

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Ensure SQL Server is running
   - Check connection string in appsettings.json
   - Verify SQL Server authentication settings

2. **JWT Token Issues**
   - Check JWT configuration in appsettings.json
   - Verify token expiration settings
   - Ensure correct Authorization header format

3. **CORS Issues**
   - Check CORS configuration in Program.cs
   - Verify frontend API URL configuration

4. **Docker Issues**
   - Ensure Docker Desktop is running
   - Check port conflicts (5000, 1433, 3000)
   - Verify Docker Compose configuration

### Logs and Debugging

- **Application Logs**: `logs/student-management-.log`
- **Console Output**: Check terminal for real-time logs
- **Swagger UI**: http://localhost:5000/swagger for API testing
- **Browser DevTools**: Network tab for API request debugging

## Project Structure

```
Zest_India_Assignment/
 StudentManagement.Api/           # .NET Core Web API
   Controllers/                  # API Controllers
   Services/                     # Business Logic
   Repositories/                 # Data Access
   Data/                         # Database Context
   Models/                       # Data Models
   Middleware/                   # Custom Middleware
   Program.cs                    # Application Entry Point
 StudentManagement.Tests/        # Unit Tests
   Services/                     # Test Classes
 StudentManagement.Frontend/     # React Frontend
   src/
     components/                  # React Components
     services/                    # API Services
   public/                        # Static Files
 docker-compose.yml              # Docker Configuration
 README.md                        # This File
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## License

This project is for educational purposes as part of the Zest India IT Pvt Ltd Technical Assignment.

## Evaluation Criteria Met

- [x] **Code Quality** - Clean, maintainable code with proper architecture
- [x] **Architecture** - Layered architecture with separation of concerns
- [x] **Error Handling** - Global exception handling middleware
- [x] **Security** - JWT authentication and secure API endpoints
- [x] **API Functionality** - Complete CRUD operations
- [x] **Unit Testing** - Comprehensive test coverage
- [x] **Docker** - Containerized deployment
- [x] **Frontend** - Modern React UI with full functionality

## Support

For any questions or issues, please refer to the troubleshooting section or create an issue in the repository.
