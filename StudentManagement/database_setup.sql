-- ============================================================
-- Student Management System - SQL Server Setup Script
-- Run this if you prefer manual DB setup over EF migrations
-- ============================================================

-- Create database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentManagementDB')
BEGIN
    CREATE DATABASE StudentManagementDB;
    PRINT 'Database StudentManagementDB created.';
END
GO

USE StudentManagementDB;
GO

-- ── Students Table ──────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type = 'U')
BEGIN
    CREATE TABLE Students (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(100)  NOT NULL,
        Email       NVARCHAR(150)  NOT NULL,
        Age         INT            NOT NULL CHECK (Age BETWEEN 1 AND 120),
        Course      NVARCHAR(100)  NOT NULL,
        CreatedDate DATETIME2      NOT NULL DEFAULT GETUTCDATE()
    );

    CREATE UNIQUE INDEX IX_Students_Email ON Students(Email);
    PRINT 'Table Students created.';
END
GO

-- ── Users Table ─────────────────────────────────────────────
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type = 'U')
BEGIN
    CREATE TABLE Users (
        Id           INT IDENTITY(1,1) PRIMARY KEY,
        Username     NVARCHAR(100) NOT NULL,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Role         NVARCHAR(50)  NOT NULL DEFAULT 'User'
    );

    CREATE UNIQUE INDEX IX_Users_Username ON Users(Username);
    PRINT 'Table Users created.';
END
GO

-- ── Sample Data ─────────────────────────────────────────────
-- NOTE: The admin password below is 'Admin@123' (BCrypt hashed)
-- You can register users via the /api/auth/register endpoint instead.

INSERT INTO Students (Name, Email, Age, Course, CreatedDate) VALUES
    ('Rahul Sharma',  'rahul.sharma@example.com',  21, 'B.Tech Computer Science', GETUTCDATE()),
    ('Priya Patel',   'priya.patel@example.com',   22, 'MBA Finance',             GETUTCDATE()),
    ('Amit Kumar',    'amit.kumar@example.com',    20, 'BCA',                     GETUTCDATE()),
    ('Sneha Desai',   'sneha.desai@example.com',   23, 'MCA',                     GETUTCDATE()),
    ('Rohit Verma',   'rohit.verma@example.com',   19, 'B.Sc Data Science',       GETUTCDATE());
GO

PRINT 'Sample student data inserted.';
PRINT 'Setup complete. Run the API and visit http://localhost:5000 for Swagger UI.';
GO
