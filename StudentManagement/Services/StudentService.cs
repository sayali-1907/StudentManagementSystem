using StudentManagement.DTOs;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
    {
        _logger.LogInformation("Fetching all students");
        var students = await _studentRepository.GetAllAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        _logger.LogInformation("Fetching student with ID: {Id}", id);
        var student = await _studentRepository.GetByIdAsync(id);
        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto dto)
    {
        _logger.LogInformation("Creating student with email: {Email}", dto.Email);

        var existing = await _studentRepository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException($"A student with email '{dto.Email}' already exists.");

        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age,
            Course = dto.Course,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _studentRepository.CreateAsync(student);
        _logger.LogInformation("Student created with ID: {Id}", created.Id);
        return MapToDto(created);
    }

    public async Task<StudentResponseDto?> UpdateStudentAsync(int id, UpdateStudentDto dto)
    {
        _logger.LogInformation("Updating student with ID: {Id}", id);

        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null) return null;

        // Check email uniqueness (excluding current student)
        var emailOwner = await _studentRepository.GetByEmailAsync(dto.Email);
        if (emailOwner != null && emailOwner.Id != id)
            throw new InvalidOperationException($"Email '{dto.Email}' is already used by another student.");

        student.Name = dto.Name;
        student.Email = dto.Email;
        student.Age = dto.Age;
        student.Course = dto.Course;

        var updated = await _studentRepository.UpdateAsync(student);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        _logger.LogInformation("Deleting student with ID: {Id}", id);
        return await _studentRepository.DeleteAsync(id);
    }

    private static StudentResponseDto MapToDto(Student student) => new()
    {
        Id = student.Id,
        Name = student.Name,
        Email = student.Email,
        Age = student.Age,
        Course = student.Course,
        CreatedDate = student.CreatedDate
    };
}
