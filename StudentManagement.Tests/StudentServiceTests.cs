using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;
using StudentManagement.Services;

namespace StudentManagement.Tests;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _repoMock;
    private readonly Mock<ILogger<StudentService>> _loggerMock;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _repoMock = new Mock<IStudentRepository>();
        _loggerMock = new Mock<ILogger<StudentService>>();
        _service = new StudentService(_repoMock.Object, _loggerMock.Object);
    }

    // ── GetAll ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllStudentsAsync_ShouldReturnMappedDtos()
    {
        var students = new List<Student>
        {
            new() { Id = 1, Name = "Alice", Email = "alice@test.com", Age = 20, Course = "CS", CreatedDate = DateTime.UtcNow },
            new() { Id = 2, Name = "Bob",   Email = "bob@test.com",   Age = 22, Course = "MBA", CreatedDate = DateTime.UtcNow }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

        var result = await _service.GetAllStudentsAsync();

        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Alice");
    }

    // ── GetById ─────────────────────────────────────────────

    [Fact]
    public async Task GetStudentByIdAsync_ExistingId_ShouldReturnDto()
    {
        var student = new Student { Id = 1, Name = "Alice", Email = "alice@test.com", Age = 20, Course = "CS" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

        var result = await _service.GetStudentByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Alice");
    }

    [Fact]
    public async Task GetStudentByIdAsync_NonExistingId_ShouldReturnNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _service.GetStudentByIdAsync(99);

        result.Should().BeNull();
    }

    // ── Create ──────────────────────────────────────────────

    [Fact]
    public async Task CreateStudentAsync_NewEmail_ShouldCreateAndReturnDto()
    {
        var dto = new CreateStudentDto { Name = "Alice", Email = "alice@test.com", Age = 20, Course = "CS" };

        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((Student?)null);
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Student>()))
            .ReturnsAsync((Student s) => { s.Id = 1; return s; });

        var result = await _service.CreateStudentAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Email.Should().Be("alice@test.com");
    }

    [Fact]
    public async Task CreateStudentAsync_DuplicateEmail_ShouldThrowInvalidOperationException()
    {
        var dto = new CreateStudentDto { Name = "Alice", Email = "alice@test.com", Age = 20, Course = "CS" };
        var existing = new Student { Id = 1, Email = "alice@test.com" };

        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(existing);

        await _service.Invoking(s => s.CreateStudentAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }

    // ── Update ──────────────────────────────────────────────

    [Fact]
    public async Task UpdateStudentAsync_ExistingStudent_ShouldUpdateAndReturnDto()
    {
        var existing = new Student { Id = 1, Name = "Alice", Email = "alice@test.com", Age = 20, Course = "CS" };
        var dto = new UpdateStudentDto { Name = "Alice Updated", Email = "alice@test.com", Age = 21, Course = "MBA" };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Student>())).ReturnsAsync((Student s) => s);

        var result = await _service.UpdateStudentAsync(1, dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Alice Updated");
        result.Age.Should().Be(21);
    }

    [Fact]
    public async Task UpdateStudentAsync_NonExistingStudent_ShouldReturnNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student?)null);

        var result = await _service.UpdateStudentAsync(99, new UpdateStudentDto());

        result.Should().BeNull();
    }

    // ── Delete ──────────────────────────────────────────────

    [Fact]
    public async Task DeleteStudentAsync_ExistingId_ShouldReturnTrue()
    {
        _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteStudentAsync(1);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteStudentAsync_NonExistingId_ShouldReturnFalse()
    {
        _repoMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

        var result = await _service.DeleteStudentAsync(99);

        result.Should().BeFalse();
    }
}
