using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for employee operations.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Gets an employee by ID.
    /// </summary>
    Task<EmployeeProfile?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all employees.
    /// </summary>
    Task<List<EmployeeProfile>> GetAllAsync();

    /// <summary>
    /// Updates an employee profile.
    /// </summary>
    Task<bool> UpdateAsync(EmployeeProfile employee);

    /// <summary>
    /// Checks if email exists for another employee.
    /// </summary>
    Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId);

    /// <summary>
    /// Checks if cell phone exists for another employee.
    /// </summary>
    Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId);
}
