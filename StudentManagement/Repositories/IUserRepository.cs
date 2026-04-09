using StudentManagement.Models;

namespace StudentManagement.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
    Task<bool> UsernameExistsAsync(string username);
}
