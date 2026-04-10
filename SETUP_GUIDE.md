# Setup Guide for Student Management System

Since Docker and .NET are not currently installed on your system, here are the complete setup instructions:

## Prerequisites Installation

### 1. Install .NET 8.0 SDK

#### Windows:
1. Download from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Run the installer and follow the setup wizard
3. Restart your terminal/command prompt
4. Verify installation:
   ```bash
   dotnet --version
   ```

#### Alternative: Using Visual Studio
1. Install Visual Studio 2022 (Community Edition is free)
2. During installation, select ".NET desktop development" workload
3. This automatically installs .NET 8.0 SDK

### 2. Install SQL Server

#### Option A: SQL Server Express (Recommended)
1. Download from: https://www.microsoft.com/sql-server/sql-server-downloads
2. Select "Express" edition
3. Install with default settings
4. Note the instance name (usually SQLEXPRESS)

#### Option B: LocalDB (Lightweight)
1. Install Visual Studio 2022 with "Data storage and processing" workload
2. LocalDB is automatically installed
3. Connection string will use `(localdb)\mssqllocaldb`

### 3. Install Node.js (for React frontend)
1. Download from: https://nodejs.org/
2. Install the LTS version
3. Restart your terminal
4. Verify installation:
   ```bash
   node --version
   npm --version
   ```

### 4. Install Docker Desktop (Optional)
1. Download from: https://www.docker.com/products/docker-desktop/
2. Install and restart your computer
3. Start Docker Desktop
4. Test installation:
   ```bash
   docker --version
   docker compose version
   ```

## Running the Application

### Option 1: Local Development (Recommended)

#### Backend Setup:
1. Open Command Prompt/PowerShell as Administrator
2. Navigate to the project directory:
   ```bash
   cd c:\Users\divya\Desktop\Zest_India_Assignment\StudentManagement.Api
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Update connection string in `appsettings.json` if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```
5. Run the API:
   ```bash
   dotnet run
   ```
6. The API will be available at: http://localhost:5000
7. Access Swagger documentation at: http://localhost:5000/swagger

#### Frontend Setup:
1. Open a NEW terminal window
2. Navigate to the frontend directory:
   ```bash
   cd c:\Users\divya\Desktop\Zest_India_Assignment\StudentManagement.Frontend
   ```
3. Install dependencies:
   ```bash
   npm install
   ```
4. Start the React app:
   ```bash
   npm start
   ```
5. The frontend will open at: http://localhost:3000

### Option 2: Using Visual Studio

#### Backend:
1. Open `StudentManagement.Api\StudentManagement.Api.csproj` in Visual Studio
2. Right-click the project and select "Set as Startup Project"
3. Press F5 or click "Start Debugging"
4. The API will start automatically

#### Frontend:
1. Open a separate terminal
2. Navigate to `StudentManagement.Frontend`
3. Run `npm install` and `npm start`

### Option 3: Docker (After Installation)

1. Install Docker Desktop (see prerequisites above)
2. Open terminal in project root:
   ```bash
   cd c:\Users\divya\Desktop\Zest_India_Assignment
   ```
3. Run:
   ```bash
   docker compose up --build
   ```

## Troubleshooting

### Common Issues:

#### 1. "dotnet command not found"
- Install .NET 8.0 SDK
- Restart your terminal
- Add .NET to PATH if needed

#### 2. SQL Server Connection Issues
- Verify SQL Server is running
- Check connection string format
- For LocalDB: Use `(localdb)\mssqllocaldb`
- For SQL Express: Use `.\SQLEXPRESS`

#### 3. Port Conflicts
- API default port: 5000
- Frontend default port: 3000
- Change ports if needed in configuration files

#### 4. Permission Issues
- Run terminal as Administrator
- Check firewall settings

#### 5. Node.js Issues
- Clear npm cache: `npm cache clean --force`
- Delete node_modules and reinstall
- Use Node.js 16+ version

## Testing the Application

### 1. Test API with Swagger
1. Navigate to http://localhost:5000/swagger
2. Use the "Try it out" feature to test endpoints
3. Login first with demo credentials
4. Copy the JWT token for authenticated requests

### 2. Test Frontend
1. Navigate to http://localhost:3000
2. Login with demo credentials:
   - Admin: admin@studentmanagement.com / Admin123!
   - Teacher: teacher@studentmanagement.com / Teacher123!
   - User: user@studentmanagement.com / User123!
3. Test CRUD operations

### 3. Run Unit Tests
1. Backend tests:
   ```bash
   cd StudentManagement.Tests
   dotnet test
   ```
2. Frontend tests:
   ```bash
   cd StudentManagement.Frontend
   npm test
   ```

## Project Structure Verification

Ensure you have this structure:
```
Zest_India_Assignment/
 StudentManagement.Api/
   Controllers/
   Services/
   Repositories/
   Data/
   Models/
   Middleware/
   Program.cs
   appsettings.json
 StudentManagement.Tests/
   Services/
 StudentManagement.Frontend/
   src/
     components/
     services/
   package.json
 docker-compose.yml
 README.md
 SETUP_GUIDE.md
```

## Next Steps

1. Install the required prerequisites
2. Choose your preferred running method
3. Test the application
4. Explore the features
5. Upload to GitHub when ready

## Support

If you encounter issues:
1. Check this guide first
2. Verify all prerequisites are installed
3. Ensure correct versions (.NET 8.0, Node.js 16+)
4. Check terminal permissions
5. Review error messages carefully

The application is fully functional once the prerequisites are installed and configured properly.
