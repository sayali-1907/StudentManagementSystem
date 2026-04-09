# 🎓 Student Management System
### ASP.NET Core 8 Web API — Zest India IT Pvt Ltd Technical Assignment

---

## 📋 Features

| Feature | Details |
|---|---|
| **CRUD** | Get All, Get by ID, Create, Update, Delete students |
| **JWT Auth** | Register & Login with BCrypt password hashing |
| **Architecture** | Controller → Service → Repository (3-layer) |
| **Database** | SQL Server with Entity Framework Core |
| **Logging** | Serilog (Console + Rolling File) |
| **Exception Handling** | Global middleware with structured error responses |
| **Swagger UI** | Full interactive API docs with JWT support |
| **Unit Tests** | xUnit + Moq + FluentAssertions |
| **Docker** | Dockerfile + docker-compose (API + SQL Server) |

---

## 🗂 Project Structure

```
StudentManagement/
├── Controllers/
│   ├── AuthController.cs        # POST /api/auth/register, /login
│   └── StudentsController.cs    # GET/POST/PUT/DELETE /api/students
├── Services/
│   ├── IStudentService.cs
│   ├── StudentService.cs
│   ├── IAuthService.cs
│   └── AuthService.cs           # JWT token generation
├── Repositories/
│   ├── IStudentRepository.cs
│   ├── StudentRepository.cs
│   ├── IUserRepository.cs
│   └── UserRepository.cs
├── Models/
│   ├── Student.cs               # Id, Name, Email, Age, Course, CreatedDate
│   └── User.cs
├── DTOs/
│   └── StudentDtos.cs           # Request/Response DTOs + ApiResponse<T>
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Migrations/                  # EF Core migrations
├── appsettings.json
├── Program.cs                   # DI, JWT, Swagger, Serilog config
└── database_setup.sql           # Manual SQL script (optional)

StudentManagement.Tests/
└── StudentServiceTests.cs       # 8 unit tests
```

---

## ⚙️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer edition is free)
- [Git](https://git-scm.com/)
- *(Optional)* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

---

## 🚀 Setup (Option A — Local)

### 1. Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/StudentManagement.git
cd StudentManagement
```

### 2. Configure the connection string
Open `StudentManagement/appsettings.json` and update:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
> For SQL Server with username/password:
> `Server=localhost;Database=StudentManagementDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;`

### 3. Apply database migrations
```bash
cd StudentManagement
dotnet ef database update
```
> Or run `database_setup.sql` manually in SQL Server Management Studio (SSMS).

### 4. Run the API
```bash
dotnet run
```
The API will start at `http://localhost:5000`.  
Swagger UI opens automatically at **http://localhost:5000** (root).

---

## 🐳 Setup (Option B — Docker)

```bash
# Build and start API + SQL Server together
docker-compose up --build

# API available at http://localhost:5000
# Swagger UI at http://localhost:5000
```

---

## 🔐 Authentication Flow

### Step 1 — Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123",
  "role": "Admin"
}
```

### Step 2 — Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}
```
Response:
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGci...",
    "username": "admin",
    "role": "Admin",
    "expiry": "2024-01-02T00:00:00Z"
  }
}
```

### Step 3 — Use Token
Add to all student API requests:
```
Authorization: Bearer eyJhbGci...
```
In Swagger UI, click **Authorize 🔒** and paste your token.

---

## 📡 API Endpoints

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | ❌ | Register new user |
| POST | `/api/auth/login` | ❌ | Login and get JWT token |
| GET | `/api/students` | ✅ | Get all students |
| GET | `/api/students/{id}` | ✅ | Get student by ID |
| POST | `/api/students` | ✅ | Create new student |
| PUT | `/api/students/{id}` | ✅ | Update student |
| DELETE | `/api/students/{id}` | ✅ | Delete student |

### Sample — Create Student
```http
POST /api/students
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Rahul Sharma",
  "email": "rahul@example.com",
  "age": 21,
  "course": "B.Tech Computer Science"
}
```

### Standard Response Format
```json
{
  "success": true,
  "message": "Student created successfully",
  "data": {
    "id": 1,
    "name": "Rahul Sharma",
    "email": "rahul@example.com",
    "age": 21,
    "course": "B.Tech Computer Science",
    "createdDate": "2024-01-01T10:00:00Z"
  }
}
```

---

## 🧪 Running Unit Tests

```bash
cd StudentManagement.Tests
dotnet test
```

Tests cover:
- GetAll returns mapped DTOs
- GetById with existing and non-existing IDs
- Create with new email (success)
- Create with duplicate email (throws exception)
- Update existing and non-existing students
- Delete existing and non-existing students

---

## 🗄 Database Schema

```sql
-- Students
CREATE TABLE Students (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL,
    Email       NVARCHAR(150) NOT NULL UNIQUE,
    Age         INT           NOT NULL,
    Course      NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2     NOT NULL DEFAULT GETUTCDATE()
);

-- Users
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Username     NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role         NVARCHAR(50)  NOT NULL DEFAULT 'User'
);
```

---

## 🛡️ Security

- Passwords hashed using **BCrypt** (cost factor 11)
- JWT tokens signed with **HMAC-SHA256**
- All student endpoints require **Bearer token**
- Global exception handler hides stack traces in production

---

## 📝 Logs

Logs are written to:
- **Console** — real-time output
- **`logs/studentmanagement-YYYYMMDD.log`** — rolling daily files

---

## 👨‍💻 Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8 (SQL Server)
- JWT Bearer Authentication
- BCrypt.Net password hashing
- Serilog logging
- Swagger / Swashbuckle
- xUnit + Moq + FluentAssertions (tests)
- Docker + Docker Compose
