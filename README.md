# Student Management System

## Overview
This is a Student Management System built using ASP.NET Core Web API. The application provides functionality to manage student records with secure authentication using JWT.

## Features
- CRUD operations for students (Create, Read, Update, Delete)
- JWT-based authentication (Register and Login)
- Layered architecture (Controller, Service, Repository)
- Entity Framework Core with SQL Server
- Global exception handling middleware
- Logging using Serilog
- Swagger UI for API testing
- Unit testing using xUnit and Moq

## Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Serilog
- Swagger
- xUnit, Moq, FluentAssertions

## Prerequisites
Make sure the following are installed:
- .NET 8 SDK
- SQL Server (Express or Developer Edition)
- SQL Server Management Studio (optional)
- Visual Studio Code
- Git

---

## How to Run the Project in VS Code

### Step 1: Clone the repository
```bash
git clone https://github.com/yourusername/StudentManagementSystem.git
cd StudentManagementSystem
```

### Step 2: Open project in VS Code
- Open VS Code
- Click on "Open Folder"
- Select the project folder

### Step 3: Configure database connection
Open `StudentManagement/appsettings.json` and update the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
If you use SQL Server with username and password:
```
Server=localhost;Database=StudentManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

### Step 4: Apply database migrations
Open terminal in VS Code and run:
```bash
cd StudentManagement
dotnet ef database update
```
This will automatically create the database and all required tables.

### Step 5: Run the application
```bash
dotnet run
```

### Step 6: Open in browser
Go to:
```
http://localhost:5000
```
Swagger UI will open where you can test all the APIs.

---

## Running with Docker (Optional)

If you have Docker Desktop installed, you can run the entire project with a single command:
```bash
docker-compose up --build
```
This will automatically start both the API and SQL Server together. No manual database setup needed.

API available at `http://localhost:5000`

---

## Authentication Flow

### Register
```
POST /api/auth/register
```
```json
{
  "username": "admin",
  "password": "Admin@123",
  "role": "Admin"
}
```

### Login
```
POST /api/auth/login
```
```json
{
  "username": "admin",
  "password": "Admin@123"
}
```
After login, copy the token from the response and click **Authorize** in Swagger UI to access the student endpoints.

---

## API Endpoints

| Method | Endpoint | Auth Required |
|---|---|---|
| POST | /api/auth/register | No |
| POST | /api/auth/login | No |
| GET | /api/students | Yes |
| GET | /api/students/{id} | Yes |
| POST | /api/students | Yes |
| PUT | /api/students/{id} | Yes |
| DELETE | /api/students/{id} | Yes |

---

## Running Unit Tests

```bash
cd StudentManagement.Tests
dotnet test
```

---

## Project Structure

```
StudentManagement/
├── Controllers/
├── Services/
├── Repositories/
├── Models/
├── DTOs/
├── Data/
├── Middleware/
└── Migrations/

StudentManagement.Tests/
└── StudentServiceTests.cs
```
