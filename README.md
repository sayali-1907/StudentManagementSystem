# Student Management System

## Overview
This is a Student Management System built using ASP.NET Core Web API. The application provides functionality to manage student records with secure authentication using JWT.

---

## Features
- CRUD operations for students (Create, Read, Update, Delete)
- JWT-based authentication (Register and Login)
- Layered architecture (Controller, Service, Repository)
- Entity Framework Core with SQL Server
- Global exception handling middleware
- Logging using Serilog
- Swagger UI for API testing
- Unit testing using xUnit and Moq

---

## Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Serilog
- Swagger
- xUnit, Moq, FluentAssertions

---

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

Step 2: Open project in VS Code
Open VS Code
Click on "Open Folder"
Select the project folder
Step 3: Configure database connection

Open appsettings.json and update the connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
Step 4: Apply database migrations

Open terminal in VS Code and run:

cd StudentManagement
dotnet ef database update
Step 5: Run the application
dotnet run
Step 6: Open in browser

Go to:

http://localhost:5000

Swagger UI will open where you can test APIs.

Authentication Flow
Register

POST /api/auth/register

Login

POST /api/auth/login

After login, copy the token and use it in Swagger Authorization.

API Endpoints
GET /api/students
GET /api/students/{id}
POST /api/students
PUT /api/students/{id}
DELETE /api/students/{id}

## Running Unit Tests
cd StudentManagement.Tests
dotnet test
